using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void Save(PlayerData playerData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static PlayerData Load()
    {
        string path = Application.persistentDataPath + "/save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            return formatter.Deserialize(stream) as PlayerData;
        }
        else
        {
            PlayerData playerData = new PlayerData();
            Save(playerData);

            return playerData;
        }
    }
}
