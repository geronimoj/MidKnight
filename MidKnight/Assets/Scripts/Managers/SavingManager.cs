using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(GameManager))]
public class SavingManager : MonoBehaviour
{
    public List<RestPoint> RestPoints = new List<RestPoint>();
    public int currentRestPoint = 0;
    public string filenameBinary = "SaveBinary.bin";
    public string filenameTxt = "SaveText.txt";
    private PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        player.transform.position = RestPoints[currentRestPoint].spawnPoint;
    }

    public bool Save(bool binary, List<Entities> entitiesToNotRespawnToSave, Dictionary<string, bool> unlocksToSave)
    {
        if (binary)
        {
            return SaveBinary(entitiesToNotRespawnToSave, unlocksToSave);
        }
        else
        {
            return SaveTxt(entitiesToNotRespawnToSave, unlocksToSave);
        }
    }

    private bool SaveBinary(List<Entities> entitiesToNotRespawnToSave, Dictionary<string, bool> unlocksToSave)
    {
        try
        {
            BinaryWriter writer = new BinaryWriter(File.OpenWrite(filenameBinary));
            writer.Write("Rest");
            writer.Write(currentRestPoint);
            writer.Write("EntitiesToNotRespawn");
            writer.Write(entitiesToNotRespawnToSave.Count);

            for (int i = 0; i < entitiesToNotRespawnToSave.Count; i++)
            {
                writer.Write(entitiesToNotRespawnToSave[i].index);
                writer.Write(entitiesToNotRespawnToSave[i].thisRoom);
            }

            writer.Write("Unlocks");
            writer.Write(unlocksToSave.Count);

            foreach (KeyValuePair<string, bool> kvp in unlocksToSave)
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

    private bool SaveTxt(List<Entities> entitiesToNotRespawnToSave, Dictionary<string, bool> unlocksToSave)
    {
        try
        {
            StreamWriter writer = new StreamWriter(filenameTxt);
            writer.WriteLine("Rest");
            writer.WriteLine(currentRestPoint);
            writer.WriteLine("EntitiesToNotRespawn");
            writer.WriteLine(entitiesToNotRespawnToSave.Count);

            for (int i = 0; i < entitiesToNotRespawnToSave.Count; i++)
            {
                writer.WriteLine(entitiesToNotRespawnToSave[i].index);
                writer.WriteLine(entitiesToNotRespawnToSave[i].thisRoom);
            }

            writer.WriteLine("Unlocks");
            writer.WriteLine(unlocksToSave.Count);

            foreach(KeyValuePair<string, bool> kvp in unlocksToSave)
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

    public bool Load(bool binary, ref List<Entities> entitiesToNotRespawnToLoad, ref Dictionary<string, bool> unlocksToLoad)
    {
        if (binary)
        {
            return LoadBinary(ref entitiesToNotRespawnToLoad, ref unlocksToLoad);
        }
        else
        {
            return LoadTxt(ref entitiesToNotRespawnToLoad, ref unlocksToLoad);
        }
    }

    private bool LoadBinary(ref List<Entities> entitiesToNotRespawnToLoad, ref Dictionary<string, bool> unlocksToLoad)
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

                if (readLine == "Rest")
                {
                    currentRestPoint = reader.ReadInt32();
                }
                else if (readLine == "EntitiesToNotRespawn")
                {
                    int count = reader.ReadInt32();

                    for (int i = 0; i < count; i++)
                    {
                        Entities tempEntity = new Entities(reader.ReadInt32(), reader.ReadString());
                        tempEntities.Add(tempEntity);
                    }

                    entitiesToNotRespawnToLoad = tempEntities;
                }
                else if (readLine == "Unlocks")
                {
                    int count = reader.ReadInt32();

                    for (int i = 0; i < count; i++)
                    {
                        tempUnlocks.Add(reader.ReadString(), reader.ReadBoolean());
                    }

                    unlocksToLoad = tempUnlocks;
                    done = true;
                }
            }

            return true;
        }
        catch (IOException ioe)
        {
            Debug.LogError($"Load failed: {ioe.Message}");
            Debug.Break();
            return false;
        }
    }

    private bool LoadTxt(ref List<Entities> entitiesToNotRespawnToLoad, ref Dictionary<string, bool> unlocksToLoad)
    {
        try
        {
            StreamReader reader = new StreamReader(filenameTxt);
            List<Entities> tempEntities = new List<Entities>();
            Dictionary<string, bool> tempUnlocks = new Dictionary<string, bool>();

            while (!reader.EndOfStream)
            {
                string readLine = reader.ReadLine();

                if (readLine == "Rest")
                {
                    currentRestPoint = int.Parse(reader.ReadLine());
                }
                else if (readLine == "EntitiesToNotRespawn")
                {
                    int count = int.Parse(reader.ReadLine());

                    for (int i = 0; i < count; i++)
                    {
                        Entities tempEntity = new Entities(int.Parse(reader.ReadLine()), reader.ReadLine());
                        tempEntities.Add(tempEntity);
                    }

                    entitiesToNotRespawnToLoad = tempEntities;
                }
                else if (readLine == "Unlocks")
                {
                    int count = int.Parse(reader.ReadLine());

                    for (int i = 0; i < count; i++)
                    {
                        tempUnlocks.Add(reader.ReadLine(), bool.Parse(reader.ReadLine()));
                    }

                    unlocksToLoad = tempUnlocks;
                }
            }

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
