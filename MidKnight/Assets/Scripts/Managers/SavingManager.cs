using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SavingManager : MonoBehaviour
{
    public string filenameTxt = "Save.txt";

    public bool SaveTxt(List<Entities> entitiesToNotRespawnToSave, Dictionary<string, bool> unlocksToSave)
    {
        try
        {
            StreamWriter writer = new StreamWriter(filenameTxt);
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

    public bool LoadTxt(ref List<Entities> entitiesToNotRespawnToLoad, ref Dictionary<string, bool> unlocksToLoad)
    {
        try
        {
            StreamReader reader = new StreamReader(filenameTxt);
            List<Entities> tempEntities = new List<Entities>();
            Dictionary<string, bool> tempUnlocks = new Dictionary<string, bool>();

            while (!reader.EndOfStream)
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
            Debug.Break();
            return false;
        }
    }
}
