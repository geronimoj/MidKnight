using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SavingManager : MonoBehaviour
{
    public bool SaveTxt(string filename, List<Entities> entitiesToNotRespawnToSave, Dictionary<string, bool> unlocksToSave)
    {
        try
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine("EntitiesToNotRespawn");
            writer.WriteLine(entitiesToNotRespawnToSave.Count);
            writer.WriteLine(entitiesToNotRespawnToSave);
            writer.WriteLine("Unlocks");
            writer.WriteLine(unlocksToSave.Count);
            writer.WriteLine(unlocksToSave);
            writer.Close();
            return true;
        }
        catch (IOException ioe)
        {
            Debug.LogError($"Save failed: {ioe.Message}");
            return false;
        }
    }

    public bool LoadTxt(string filename, ref List<Entities> entitiesToNotRespawnToLoad, ref Dictionary<string, bool> unlocksToLoad)
    {
        try
        {
            StreamReader reader = new StreamReader(filename);
            List<Entities> tempEntities = new List<Entities>();
            Dictionary<string, bool> tempUnlocks = new Dictionary<string, bool>();
            bool done = false;

            while (!done)
            {
                string readLine = reader.ReadLine();

                if (readLine == "EntitiesToNotRespawn")
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
            return false;
        }
    }
}
