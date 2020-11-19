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

    private void Start()
    {
        EM = GetComponent<EntitiesManager>();
        player = FindObjectOfType<PlayerController>();
        player.transform.position = RestPoints[currentRestPoint].spawnPoint;
    }

    public bool Save(bool binary)
    {
        if (binary)
        {
            return SaveBinary();
        }
        else
        {
            return SaveTxt();
        }
    }

    private bool SaveBinary()
    {
        try
        {
            BinaryWriter writer = new BinaryWriter(File.OpenWrite(filenameBinary));
            writer.Write("Player");
            writer.Write(player.MaxHealth);
            writer.Write(currentRestPoint);
            writer.Write("EntitiesToNotRespawn");
            writer.Write(EM.EntitiesToNeverRespawn.Count);
            writer.Write(EM.EntitiesToNotRespawnUntillRest.Count);

            for (int i = 0; i < EM.EntitiesToNeverRespawn.Count; i++)
            {
                writer.Write(EM.EntitiesToNeverRespawn[i].index);
                writer.Write(EM.EntitiesToNeverRespawn[i].thisRoom);
            }
            for (int i = 0; i < EM.EntitiesToNotRespawnUntillRest.Count; i++)
            {
                writer.Write(EM.EntitiesToNotRespawnUntillRest[i].index);
                writer.Write(EM.EntitiesToNotRespawnUntillRest[i].thisRoom);
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

    private bool SaveTxt()
    {
        try
        {
            StreamWriter writer = new StreamWriter(filenameTxt);
            writer.WriteLine("Player");
            writer.WriteLine(player.MaxHealth);
            writer.WriteLine(currentRestPoint);
            writer.WriteLine("EntitiesToNotRespawn");
            writer.WriteLine(EM.EntitiesToNeverRespawn.Count);
            writer.WriteLine(EM.EntitiesToNotRespawnUntillRest.Count);

            for (int i = 0; i < EM.EntitiesToNeverRespawn.Count; i++)
            {
                writer.WriteLine(EM.EntitiesToNeverRespawn[i].index);
                writer.WriteLine(EM.EntitiesToNeverRespawn[i].thisRoom);
            }
            for (int i = 0; i < EM.EntitiesToNotRespawnUntillRest.Count; i++)
            {
                writer.WriteLine(EM.EntitiesToNotRespawnUntillRest[i].index);
                writer.WriteLine(EM.EntitiesToNotRespawnUntillRest[i].thisRoom);
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

    public bool Load(bool binary)
    {
        if (binary)
        {
            return LoadBinary();
        }
        else
        {
            return LoadTxt();
        }
    }

    private bool LoadBinary()
    {
        try
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(filenameBinary));
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
                }
                else if (readLine == "EntitiesToNotRespawn")
                {
                    int NeverCount = reader.ReadInt32();
                    int NotCount = reader.ReadInt32();

                    for (int i = 0; i < NeverCount; i++)
                    {
                        Entities tempEntity = new Entities(reader.ReadInt32(), reader.ReadString());
                        tempEntities.Add(tempEntity);
                    }

                    EM.EntitiesToNeverRespawn = tempEntities;
                    tempEntities.Clear();

                    for (int i = 0; i < NotCount; i++)
                    {
                        Entities tempEntity = new Entities(reader.ReadInt32(), reader.ReadString());
                        tempEntities.Add(tempEntity);
                    }

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

                    player.GetComponent<UnlockTracker>().unlocks = tempUnlocks;
                    done = true;
                }
            }

            reader.Close();
            return true;
        }
        catch (IOException ioe)
        {
            Debug.LogError($"Load failed: {ioe.Message}");
            Debug.Break();
            return false;
        }
    }

    private bool LoadTxt()
    {
        try
        {
            StreamReader reader = new StreamReader(filenameTxt);
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
                }
                else if (readLine == "EntitiesToNotRespawn")
                {
                    int NeverCount = int.Parse(reader.ReadLine());
                    int NotCount = int.Parse(reader.ReadLine());

                    for (int i = 0; i < NeverCount; i++)
                    {
                        Entities tempEntity = new Entities(int.Parse(reader.ReadLine()), reader.ReadLine());
                        tempEntities.Add(tempEntity);
                    }

                    EM.EntitiesToNeverRespawn = tempEntities;
                    tempEntities.Clear();

                    for (int i = 0; i < NotCount; i++)
                    {
                        Entities tempEntity = new Entities(int.Parse(reader.ReadLine()), reader.ReadLine());
                        tempEntities.Add(tempEntity);
                    }

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

                    player.GetComponent<UnlockTracker>().unlocks = tempUnlocks;
                }
            }

            reader.Close();
            return true;
        }
        catch (IOException ioe)
        {
            Debug.LogError($"Load failed: {ioe.Message}");
            Debug.Break();
            return false;
        }
    }
}

[System.Serializable]
public struct RestPoint
{
    public Vector3 spawnPoint;
    public string thisRoom;
}
