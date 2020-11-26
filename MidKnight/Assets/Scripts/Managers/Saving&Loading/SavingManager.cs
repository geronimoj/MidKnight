using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(GameManager))]
public class SavingManager : MonoBehaviour
{
    public List<RestPoint> RestPoints = new List<RestPoint>();
    public int currentRestPoint = 0;
    [SerializeField]
    private string filenameBinary = "SaveBinary.bin";
    [SerializeField]
    private string filenameTxt = "SaveText.txt";
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private EntitiesManager EM;
    [SerializeField]
    private GameManager GM;

    public bool loadRoomFinished = false;
    public bool loadRoomStarted = false;

    private void Start()
    {
        EM = GetComponent<EntitiesManager>();
        GM = GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.transform.position = RestPoints[currentRestPoint].spawnPoint;
    }

    private void Update()
    {
        if (player.Dead)
        {
            if (!loadRoomStarted)
                StartCoroutine(LoadRestRoom());
            if (loadRoomFinished)
                player.Dead = false;
        }
    }

    public bool Save(bool binary = false, string filename = "")
    {
        if (filename == "")
        {
            filename = binary ? filenameBinary : filenameTxt;
        }
        if (binary)
        {
            return SaveBinary(filename);
        }
        else
        {
            return SaveTxt(filename);
        }
    }

    private bool SaveBinary(string filename)
    {
        try
        {
            BinaryWriter writer = new BinaryWriter(File.OpenWrite(filename));
            writer.Write("Player");
            writer.Write(player.MaxHealth);
            writer.Write(player.MoonLight);
            writer.Write(currentRestPoint);
            writer.Write(player.GetComponent<PhaseManager>().KnownPhases.Count);

            for (int i = 0; i < player.GetComponent<PhaseManager>().KnownPhases.Count; i++)
            {
                writer.Write(player.GetComponent<PhaseManager>().KnownPhases[i].phaseID);
            }

            writer.Write("EntitiesToNeverRespawn");
            writer.Write(EM.EntitiesToNeverRespawn.Count);

            for (int i = 0; i < EM.EntitiesToNeverRespawn.Count; i++)
            {
                writer.Write(EM.EntitiesToNeverRespawn[i].index);
                writer.Write(EM.EntitiesToNeverRespawn[i].thisRoom);
            }

            writer.Write("Unlocks");
            writer.Write(player.GetComponent<UnlockTracker>().HealthAdd);
            writer.Write(player.GetComponent<UnlockTracker>().EclipseAdd);
            writer.Write(player.GetComponent<UnlockTracker>().unlocks.Count);

            foreach (KeyValuePair<string, bool> kvp in player.GetComponent<UnlockTracker>().unlocks)
            {
                writer.Write(kvp.Key);
                writer.Write(kvp.Value);
            }

            writer.Close();
            return true;
        }
        catch(IOException ioe)
        {
            Debug.LogError($"Save failed: {ioe.Message}");
            Debug.Break();
            return false;
        }
    }

    private bool SaveTxt(string filename)
    {
        try
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine("Player");
            writer.WriteLine(player.MaxHealth);
            writer.WriteLine(player.MoonLight);
            writer.WriteLine(currentRestPoint);
            writer.WriteLine(player.GetComponent<PhaseManager>().KnownPhases.Count);

            for (int i = 0; i < player.GetComponent<PhaseManager>().KnownPhases.Count; i++)
            {
                writer.WriteLine(player.GetComponent<PhaseManager>().KnownPhases[i].phaseID);
            }

            writer.WriteLine("EntitiesToNeverRespawn");
            writer.WriteLine(EM.EntitiesToNeverRespawn.Count);

            for (int i = 0; i < EM.EntitiesToNeverRespawn.Count; i++)
            {
                writer.WriteLine(EM.EntitiesToNeverRespawn[i].index);
                writer.WriteLine(EM.EntitiesToNeverRespawn[i].thisRoom);
            }

            writer.WriteLine("Unlocks");
            writer.WriteLine(player.GetComponent<UnlockTracker>().HealthAdd);
            writer.WriteLine(player.GetComponent<UnlockTracker>().EclipseAdd);
            writer.WriteLine(player.GetComponent<UnlockTracker>().unlocks.Count);

            foreach(KeyValuePair<string, bool> kvp in player.GetComponent<UnlockTracker>().unlocks)
            {
                writer.WriteLine(kvp.Key);
                writer.WriteLine(kvp.Value);
            }

            writer.Close();
            return true;
        }
        catch (IOException ioe)
        {
            Debug.LogError($"Save failed: {ioe.Message}");
            Debug.Break();
            return false;
        }
    }

    public bool Load(bool binary = false, bool instantiate = false, string filename = "")
    {
        bool result;

        if (filename == "")
        {
            filename = binary ? filenameBinary : filenameTxt;
        }
        if (binary)
        {
            result = LoadBinary(filename);
        }
        else
        {
            result = LoadTxt(filename);
        }
        if (instantiate && result)
        {
            RestPoints[currentRestPoint].thisRoom.InstantiateRoom(ref GM);
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = RestPoints[currentRestPoint].spawnPoint;
            player.GetComponent<CharacterController>().enabled = true;
        }

        return result;
    }

    private bool LoadBinary(string filename)
    {
        int tempMaxHealth = 4;
        int tempEndMaxHealth = 4;
        float tempMoonlight = 100;
        float tempEndMoonlight = 100;
        int tempRestPoint = 0;
        int tempEndRestPoint = 0;
        List<MoonPhase> tempEndKnownPhases = new List<MoonPhase>();
        List<Entities> tempEndEntities = new List<Entities>();
        int tempHealthAdd = 0;
        int tempEndHealthAdd = 0;
        int tempEclipseAdd = 0;
        int tempEndEclipseAdd = 0;
        Dictionary<string, bool> tempEndUnlocks = new Dictionary<string, bool>();

        try
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(filename));
            List<MoonPhase> tempKnownPhases = new List<MoonPhase>();
            List<Entities> tempEntities = new List<Entities>();
            Dictionary<string, bool> tempUnlocks = new Dictionary<string, bool>();
            bool done = false;

            while (!done)
            {
                string readLine = reader.ReadString();

                if (readLine == "Player")
                {
                    tempMaxHealth = reader.ReadInt32();
                    tempMoonlight = reader.ReadInt32();
                    tempRestPoint = reader.ReadInt32();
                    int count = reader.ReadInt32();

                    for (int i = 0; i < count; i++)
                    {
                        string phaseID = reader.ReadString(); 

                        for (int e = 0; e < player.GetComponent<PhaseManager>().everyMoonPhase.Length; e++)
                        {
                            if (phaseID == player.GetComponent<PhaseManager>().everyMoonPhase[e].phaseID)
                            {
                                tempKnownPhases.Add(player.GetComponent<PhaseManager>().everyMoonPhase[e]);
                                break;
                            }
                        }
                    }
                }
                else if (readLine == "EntitiesToNeverRespawn")
                {
                    int count = reader.ReadInt32();

                    for (int i = 0; i < count; i++)
                    {
                        Entities tempEntity = new Entities(reader.ReadInt32(), reader.ReadString());
                        tempEntities.Add(tempEntity);
                    }
                }
                else if (readLine == "Unlocks")
                {
                    tempHealthAdd = reader.ReadInt32();
                    tempEclipseAdd = reader.ReadInt32();
                    int count = reader.ReadInt32();

                    for (int i = 0; i < count; i++)
                    {
                        tempUnlocks.Add(reader.ReadString(), reader.ReadBoolean());
                    }
                    done = true;
                }
            }

            foreach (MoonPhase phase in player.GetComponent<PhaseManager>().KnownPhases)
            {
                tempEndKnownPhases.Add(phase);
            }
            foreach (Entities entity in EM.EntitiesToNeverRespawn)
            {
                tempEndEntities.Add(entity);
            }
            foreach (KeyValuePair<string, bool> KVPSB in player.GetComponent<UnlockTracker>().unlocks)
            {
                tempEndUnlocks.Add(KVPSB.Key, KVPSB.Value);
            }

            tempEndMaxHealth = player.GetComponent<PlayerController>().MaxHealth;
            tempEndMoonlight = player.GetComponent<PlayerController>().MoonLight;
            tempEndRestPoint = GM.GetComponent<SavingManager>().currentRestPoint;
            tempEndHealthAdd = player.GetComponent<UnlockTracker>().HealthAdd;
            tempEndEclipseAdd = player.GetComponent<UnlockTracker>().EclipseAdd;
            player.GetComponent<PhaseManager>().KnownPhases.Clear();
            EM.EntitiesToNeverRespawn.Clear();
            EM.EntitiesToNotRespawnUntillRest.Clear();
            player.GetComponent<UnlockTracker>().unlocks.Clear();

            foreach (MoonPhase phase in tempKnownPhases)
            {
                player.GetComponent<PhaseManager>().KnownPhases.Add(phase);
            }
            foreach (Entities entity in tempEntities)
            {
                EM.EntitiesToNeverRespawn.Add(entity);
                EM.EntitiesToNotRespawnUntillRest.Add(entity);
            }
            foreach (KeyValuePair<string, bool> KVPSB in tempUnlocks)
            {
                player.GetComponent<UnlockTracker>().unlocks.Add(KVPSB.Key, KVPSB.Value);
            }

            player.GetComponent<PlayerController>().SetMaxHealth = tempMaxHealth;
            player.TakeDamage(-player.MaxHealth);
            player.GetComponent<PlayerController>().MoonLight = tempMoonlight;
            currentRestPoint = tempRestPoint;
            player.GetComponent<UnlockTracker>().HealthAdd = tempHealthAdd;
            player.GetComponent<UnlockTracker>().EclipseAdd = tempEclipseAdd;
            reader.Close();
            return true;
        }
        catch (IOException ioe)
        {
            foreach (MoonPhase phase in tempEndKnownPhases)
            {
                player.GetComponent<PhaseManager>().KnownPhases.Add(phase);
            }
            foreach (Entities entity in tempEndEntities)
            {
                EM.EntitiesToNeverRespawn.Add(entity);
                EM.EntitiesToNotRespawnUntillRest.Add(entity);
            }
            foreach (KeyValuePair<string, bool> KVPSB in tempEndUnlocks)
            {
                player.GetComponent<UnlockTracker>().unlocks.Add(KVPSB.Key, KVPSB.Value);
            }

            player.GetComponent<PlayerController>().SetMaxHealth = tempEndMaxHealth;
            player.TakeDamage(-player.MaxHealth);
            player.GetComponent<PlayerController>().MoonLight = tempEndMoonlight;
            currentRestPoint = tempEndRestPoint;
            player.GetComponent<UnlockTracker>().HealthAdd = tempEndHealthAdd;
            player.GetComponent<UnlockTracker>().EclipseAdd = tempEndEclipseAdd;
            Debug.LogError($"Load failed: {ioe.Message}.");
            return false;
        }
    }

    private bool LoadTxt(string filename)
    {
        int tempMaxHealth = 4;
        int tempEndMaxHealth = 4;
        float tempMoonlight = 100;
        float tempEndMoonlight = 100;
        int tempRestPoint = 0;
        int tempEndRestPoint = 0;
        List<MoonPhase> tempEndKnownPhases = new List<MoonPhase>();
        List<Entities> tempEndEntities = new List<Entities>();
        int tempHealthAdd = 0;
        int tempEndHealthAdd = 0;
        int tempEclipseAdd = 0;
        int tempEndEclipseAdd = 0;
        Dictionary<string, bool> tempEndUnlocks = new Dictionary<string, bool>();

        try
        {
            StreamReader reader = new StreamReader(filename);
            List<MoonPhase> tempKnownPhases = new List<MoonPhase>();
            List<Entities> tempEntities = new List<Entities>();
            Dictionary<string, bool> tempUnlocks = new Dictionary<string, bool>();

            while (!reader.EndOfStream)
            {
                string readLine = reader.ReadLine();

                if (readLine == "Player")
                {
                    tempMaxHealth = int.Parse(reader.ReadLine());
                    tempMoonlight = int.Parse(reader.ReadLine());
                    tempRestPoint = int.Parse(reader.ReadLine());
                    int count = int.Parse(reader.ReadLine());

                    for (int i = 0; i < count; i++)
                    {
                        string phaseID = reader.ReadLine();

                        for (int e = 0; e < player.GetComponent<PhaseManager>().everyMoonPhase.Length; e++)
                        {
                            if (phaseID == player.GetComponent<PhaseManager>().everyMoonPhase[e].phaseID)
                            {
                                tempKnownPhases.Add(player.GetComponent<PhaseManager>().everyMoonPhase[e]);
                                break;
                            }
                        }
                    }
                }
                else if (readLine == "EntitiesToNeverRespawn")
                {
                    int count = int.Parse(reader.ReadLine());

                    for (int i = 0; i < count; i++)
                    {
                        Entities tempEntity = new Entities(int.Parse(reader.ReadLine()), reader.ReadLine());
                        tempEntities.Add(tempEntity);
                    }
                }
                else if (readLine == "Unlocks")
                {
                    tempHealthAdd = int.Parse(reader.ReadLine());
                    tempEclipseAdd = int.Parse(reader.ReadLine());
                    int count = int.Parse(reader.ReadLine());

                    for (int i = 0; i < count; i++)
                    {
                        tempUnlocks.Add(reader.ReadLine(), bool.Parse(reader.ReadLine()));
                    }
                }
            }

            foreach (MoonPhase phase in player.GetComponent<PhaseManager>().KnownPhases)
            {
                tempEndKnownPhases.Add(phase);
            }
            foreach (Entities entity in EM.EntitiesToNeverRespawn)
            {
                tempEndEntities.Add(entity);
            }
            foreach (KeyValuePair<string, bool> KVPSB in player.GetComponent<UnlockTracker>().unlocks)
            {
                tempEndUnlocks.Add(KVPSB.Key, KVPSB.Value);
            }

            tempEndMaxHealth = player.GetComponent<PlayerController>().MaxHealth;
            tempEndMoonlight = player.GetComponent<PlayerController>().MoonLight;
            tempEndRestPoint = GM.GetComponent<SavingManager>().currentRestPoint;
            tempEndHealthAdd = player.GetComponent<UnlockTracker>().HealthAdd;
            tempEndEclipseAdd = player.GetComponent<UnlockTracker>().EclipseAdd;
            player.GetComponent<PhaseManager>().KnownPhases.Clear();
            EM.EntitiesToNeverRespawn.Clear();
            EM.EntitiesToNotRespawnUntillRest.Clear();
            player.GetComponent<UnlockTracker>().unlocks.Clear();

            foreach (MoonPhase phase in tempKnownPhases)
            {
                player.GetComponent<PhaseManager>().KnownPhases.Add(phase);
            }
            foreach (Entities entity in tempEntities)
            {
                EM.EntitiesToNeverRespawn.Add(entity);
                EM.EntitiesToNotRespawnUntillRest.Add(entity);
            }
            foreach (KeyValuePair<string, bool> KVPSB in tempUnlocks)
            {
                player.GetComponent<UnlockTracker>().unlocks.Add(KVPSB.Key, KVPSB.Value);
            }

            player.GetComponent<PlayerController>().SetMaxHealth = tempMaxHealth;
            player.TakeDamage(-player.MaxHealth);
            player.GetComponent<PlayerController>().MoonLight = tempMoonlight;
            currentRestPoint = tempRestPoint;
            player.GetComponent<UnlockTracker>().HealthAdd = tempHealthAdd;
            player.GetComponent<UnlockTracker>().EclipseAdd = tempEclipseAdd;
            reader.Close();
            return true;
        }
        catch (IOException ioe)
        {
            foreach (MoonPhase phase in tempEndKnownPhases)
            {
                player.GetComponent<PhaseManager>().KnownPhases.Add(phase);
            }
            foreach (Entities entity in tempEndEntities)
            {
                EM.EntitiesToNeverRespawn.Add(entity);
                EM.EntitiesToNotRespawnUntillRest.Add(entity);
            }
            foreach (KeyValuePair<string, bool> KVPSB in tempEndUnlocks)
            {
                player.GetComponent<UnlockTracker>().unlocks.Add(KVPSB.Key, KVPSB.Value);
            }

            player.GetComponent<PlayerController>().SetMaxHealth = tempEndMaxHealth;
            player.TakeDamage(-player.MaxHealth);
            player.GetComponent<PlayerController>().MoonLight = tempEndMoonlight;
            currentRestPoint = tempEndRestPoint;
            player.GetComponent<UnlockTracker>().HealthAdd = tempEndHealthAdd;
            player.GetComponent<UnlockTracker>().EclipseAdd = tempEndEclipseAdd;
            Debug.LogError($"Load failed: {ioe.Message}.");
            return false;
        }
    }

    public void EnterRestPoint()
    {
        EM.EntitiesToNotRespawnUntillRest.Clear();

        foreach (Entities entity in EM.EntitiesToNeverRespawn)
        {
            EM.EntitiesToNotRespawnUntillRest.Add(entity);
        }

        //player.GetComponent<CharacterController>().enabled = false;
        player.animator.SetTrigger("EnterRest");
        player.TakeDamage(-player.GetComponent<PlayerController>().MaxHealth);
        player.transform.position = RestPoints[currentRestPoint].spawnPoint;
        player.enabled = false;
        Debug.Log("Save Text: " + Save());
        Debug.Log("Save Binary: " + Save(true));
    }

    public IEnumerator LoadRestRoom()
    {
        loadRoomStarted = true;
        loadRoomFinished = false;
        //Start fading in
        ScreenFade.ScreenFader.FadeIn();
        //Wait till we have finished fading in
        while (!ScreenFade.ScreenFader.FadeFinished())
            yield return null;
        //Destroy the room
        Destroy(GM.room.gameObject);
        //Enter the rest point
        EnterRestPoint();
        player.enabled = true;
        //Load the next room
        RestPoints[currentRestPoint].thisRoom.InstantiateRoom(ref GM);
        //Start fading back out
        ScreenFade.ScreenFader.FadeOut();
        //Wait till we have faded back out
        while (!ScreenFade.ScreenFader.FadeFinished())
            yield return null;

        loadRoomFinished = true;
        //Wait a frame before we say this has finished so loadRoomFinished can be read without loadRoomStarted being false
        yield return null;
        loadRoomStarted = false;
    }
}

[System.Serializable]
public struct RestPoint
{
    public Vector3 spawnPoint;
    public Room thisRoom;
}
