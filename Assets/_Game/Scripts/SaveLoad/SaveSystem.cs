using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Text;
using System;
using System.Threading;
using System.Runtime.Serialization;

public static class SaveSystem
{
    public static string filePath = Application.persistentDataPath;
    public static string fileName = "FridgeOrganizingSaveData.bin";
    public static string fullSavePath = Application.persistentDataPath + "/FridgeOrganizingSaveData.bin";
    public static void SaveGame(SaveData saveData)
    {
         BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(fullSavePath, FileMode.Create);

        formatter.Serialize(stream, saveData);
        stream.Close(); 

/*           DataContractSerializer bf = new DataContractSerializer(data.GetType());
        MemoryStream streamer = new MemoryStream();

        //Serialize the file
        bf.WriteObject(streamer, saveData);
        streamer.Seek(0, SeekOrigin.Begin);

        //Save to disk
        file.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);

        // Close the file to prevent any corruptions
        file.Close();

        string result = XElement.Parse(Encoding.ASCII.GetString(streamer.GetBuffer()).Replace("\0", "")).ToString();
        Debug.Log("Serialized Result: " + result); */
    }

    public static void SaveGameAsync(SaveData saveData)
    {
        Thread t = new Thread(() =>
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(fullSavePath, FileMode.Create);

            formatter.Serialize(stream, saveData);
            stream.Close();
        });

        t.Start();

    }

    public static SaveData LoadGame()
    {
        if (File.Exists(fullSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(fullSavePath, FileMode.Open);

            SaveData saveData = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return saveData;
        }
        else
        {
            Debug.LogError("Save file not found in " + fullSavePath);

            return null;
        }

    }

    /*     public static void SaveGame(SaveData saveData)
        {
            Type[] types = { typeof(ProductInShop),typeof(CompletedContainerInfo)};
            var aSerializer = new XmlSerializer(typeof(SaveData), types);
            StringBuilder sb = new StringBuilder();


            FileStream stream = new FileStream(fullSavePath, FileMode.Create);
            aSerializer.Serialize(stream, saveData); // pass an instance of A      
            stream.Close();
        }



        public static SaveData LoadGame()
        {
            if (File.Exists(fullSavePath))
            {
                var aSerializer = new XmlSerializer(typeof(SaveData));
                FileStream stream = new FileStream(fullSavePath, FileMode.Open);

                SaveData saveData = aSerializer.Deserialize(stream) as SaveData;
                stream.Close();

                return saveData;
            }
            else
            {
                return null;
            }

        } */


}
