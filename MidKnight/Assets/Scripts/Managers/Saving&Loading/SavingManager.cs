using System.Collections.Generic;
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

    private void Start()
    {
        EM = GetComponent<EntitiesManager>();
        GM = GetComponent<GameManager>();
        player = FindObjectOfType<PlayerController>();
        player.transform.position = RestPoints[currentRestPoint].spawnPoint;
    }

    private void Update()
    {
        if (player.Dead)
        {
            LoadRestRoom();
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
        if (instantiate)
        {
            RestPoints[currentRestPoint].thisRoom.InstantiateRoom(ref GM);
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = RestPoints[currentRestPoint].spawnPoint;
            player.GetComponent<CharacterController>().enabled = true;
        }

        return result;
    }

    public bool LoadDefaultBinary()
    {
        return LoadBinary("default.bin");
    }

    public bool LoadDefaultTxt()
    {
        return LoadBinary("default.txt");
    }

    private bool LoadBinary(string filename)
    {
        try
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(filename));
            List<Entities> tempEntities = new List<Entities>();
            Dictionary<string, bool> tempUnlocks = new Dictionary<string, bool>();
            bool done = false;

            while (!done)
            {
                string readLine = reader.ReadString();

                if (readLine == "Player")
                {
                    player.SetMaxHealth = reader.ReadInt32();
                    player.TakeDamage(-player.MaxHealth);
                    currentRestPoint = reader.ReadInt32();
                    int count = reader.ReadInt32();

                    for (int i = 0; i < count; i++)
                    {
                        string phaseID = reader.ReadString(); 

                        for (int e = 0; e < player.GetComponent<PhaseManager>().everyMoonPhase.Length; e++)
                        {
                            if (phaseID == player.GetComponent<PhaseManager>().everyMoonPhase[e].phaseID)
                            {
                                player.GetComponent<PhaseManager>().KnownPhases[i] = player.GetComponent<PhaseManager>().everyMoonPhase[e];
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

                    EM.EntitiesToNeverRespawn.Clear();
                    EM.EntitiesToNotRespawnUntillRest.Clear();
                    EM.EntitiesToNeverRespawn = tempEntities;
                    EM.EntitiesToNotRespawnUntillRest = tempEntities;
                }
                else if (readLine == "Unlocks")
                {
                    player.GetComponent<UnlockTracker>().HealthAdd = reader.ReadInt32();
                    player.GetComponent<UnlockTracker>().EclipseAdd = reader.ReadInt32();
                    int count = reader.ReadInt32();

                    for (int i = 0; i < count; i++)
                    {
                        tempUnlocks.Add(reader.ReadString(), reader.ReadBoolean());
                    }

                    player.GetComponent<UnlockTracker>().unlocks.Clear();
                    player.GetComponent<UnlockTracker>().unlocks = tempUnlocks;
                    done = true;
                }
            }

            reader.Close();
            return true;
        }
        catch (IOException ioe)
        {
            Debug.LogError($"Load failed: {ioe.Message}, Loading Default.");
            return LoadDefaultBinary();
        }
    }

    private bool LoadTxt(string filename)
    {
        try
        {
            StreamReader reader = new StreamReader(filename);
            List<MoonPhase> tempMoonPhases = new List<MoonPhase>();
            List<Entities> tempEntities = new List<Entities>();
            Dictionary<string, bool> tempUnlocks = new Dictionary<string, bool>();

            while (!reader.EndOfStream)
            {
                string readLine = reader.ReadLine();

                if (readLine == "Player")
                {
                    player.SetMaxHealth = int.Parse(reader.ReadLine());
                    player.TakeDamage(-player.MaxHealth);
                    currentRestPoint = int.Parse(reader.ReadLine());
                    int count = int.Parse(reader.ReadLine());

                    for (int i = 0; i < count; i++)
                    {
                        string phaseID = reader.ReadLine();

                        for (int e = 0; e < player.GetComponent<PhaseManager>().everyMoonPhase.Length; e++)
                        {
                            if (phaseID == player.GetComponent<PhaseManager>().everyMoonPhase[e].phaseID)
                            {
                                tempMoonPhases.Add(player.GetComponent<PhaseManager>().everyMoonPhase[e]);
                                break;
                            }
                        }
                    }

                    player.GetComponent<PhaseManager>().KnownPhases.Clear();
                    player.GetComponent<PhaseManager>().KnownPhases = tempMoonPhases;
                }
                else if (readLine == "EntitiesToNeverRespawn")
                {
                    int count = int.Parse(reader.ReadLine());

                    for (int i = 0; i < count; i++)
                    {
                        Entities tempEntity = new Entities(int.Parse(reader.ReadLine()), reader.ReadLine());
                        tempEntities.Add(tempEntity);
                    }

                    EM.EntitiesToNeverRespawn.Clear();
                    EM.EntitiesToNotRespawnUntillRest.Clear();
                    EM.EntitiesToNeverRespawn = tempEntities;
                    EM.EntitiesToNotRespawnUntillRest = tempEntities;
                }
                else if (readLine == "Unlocks")
                {
                    player.GetComponent<UnlockTracker>().HealthAdd = int.Parse(reader.ReadLine());
                    player.GetComponent<UnlockTracker>().EclipseAdd = int.Parse(reader.ReadLine());
                    int count = int.Parse(reader.ReadLine());

                    for (int i = 0; i < count; i++)
                    {
                        tempUnlocks.Add(reader.ReadLine(), bool.Parse(reader.ReadLine()));
                    }

                    player.GetComponent<UnlockTracker>().unlocks.Clear();
                    player.GetComponent<UnlockTracker>().unlocks = tempUnlocks;
                }
            }

            reader.Close();
            return true;
        }
        catch (IOException ioe)
        {
            Debug.LogError($"Load failed: {ioe.Message}, Loading Default.");
            return LoadDefaultTxt();
        }
    }

    public void EnterRestPoint()
    {
        EM.EntitiesToNotRespawnUntillRest.Clear();

        foreach (Entities entity in EM.EntitiesToNeverRespawn)
        {
            EM.EntitiesToNotRespawnUntillRest.Add(entity);
        }

        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerController>().TakeDamage(-player.GetComponent<PlayerController>().MaxHealth);
        player.GetComponent<PlayerController>().transform.position = RestPoints[currentRestPoint].spawnPoint;
        //Debug.Log("Save Text: " + SM.Save());
        Debug.Log("Save Binary: " + Save(true));
    }

    public void LoadRestRoom()
    {
        Destroy(GM.room.gameObject);
        EnterRestPoint();
        RestPoints[currentRestPoint].thisRoom.InstantiateRoom(ref GM);
    }
}

[System.Serializable]
public struct RestPoint
{
    public Vector3 spawnPoint;
    public Room thisRoom;
}
