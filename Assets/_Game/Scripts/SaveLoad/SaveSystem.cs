﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Text;
using System;

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
