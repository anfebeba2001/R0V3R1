
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager 
{
    public static void saveDroppenTearsData(int amount, float[] position)
    {
        DroppenTearsData droppenTearsData = new DroppenTearsData(amount,position);
        string dataPath = Application.persistentDataPath + "/droppenTears.save";
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream,droppenTearsData);
        fileStream.Close();
    }

    public static DroppenTearsData loadDroppenTearsData()
    {
        string dataPath = Application.persistentDataPath + "/droppenTears.save";
        if((File.Exists(dataPath)))
        {    
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter(); 
            fileStream.Position = 0;
            DroppenTearsData droppenTearsData = (DroppenTearsData) binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return droppenTearsData;
        }
        else 
            return null;
    }
}
