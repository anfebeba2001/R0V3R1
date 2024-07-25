
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

    public static void savePlayerData(int defense,int health, int damage, int resistance, int tears, int healingOrbs, int arrows, int costPerUpgrade)
    {
        PlayerData playerData = new PlayerData(defense,health,damage,resistance,tears,healingOrbs,arrows, costPerUpgrade);
        string dataPath = Application.persistentDataPath + "/player.save";
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream,playerData);
        fileStream.Close();
    }

    public static PlayerData loadPlayerData()
    {
        string dataPath = Application.persistentDataPath + "/player.save";
        if((File.Exists(dataPath)))
        {    
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter(); 
            fileStream.Position = 0;
            PlayerData playerData = (PlayerData) binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return playerData;
        }
        else 
            return null;
    }



}
