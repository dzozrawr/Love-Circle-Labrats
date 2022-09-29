using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Text;
using System;
using System.Threading;
using System.Runtime.Serialization;
using System.Xml.Linq;

public static class SaveSystem
{
    public static string filePath = Application.persistentDataPath;
    public static string fileName = "DatelineShow.bin";
    public static string fullSavePath = Application.persistentDataPath + "/DatelineShow.bin";
    public static void SaveGame(SaveData saveData)
    {
        /*          BinaryFormatter formatter = new BinaryFormatter();

                FileStream stream = new FileStream(fullSavePath, FileMode.Create);

                formatter.Serialize(stream, saveData);
                stream.Close();  */

        FileStream file = File.Create(fullSavePath);

        DataContractSerializer bf = new DataContractSerializer(saveData.GetType());
        MemoryStream streamer = new MemoryStream();

        //Serialize the file
        bf.WriteObject(streamer, saveData);
        streamer.Seek(0, SeekOrigin.Begin);

        //bf.ReadObject()

        string b64String = EncodeTo64(System.Text.UTF8Encoding.UTF8.GetString(streamer.GetBuffer()));

        // Debug.Log(System.Text.UTF8Encoding.UTF8.GetString(streamer.GetBuffer()));
        // Debug.Log(b64String);

        byte[] writeBuffer = System.Text.UTF8Encoding.UTF8.GetBytes(b64String);

        //Save to disk
        // File.WriteAllText(fullSavePath,b64String);
        // File.WriteAllBytes(fullSavePath,writeBuffer);
        //file.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);
        file.Write(writeBuffer, 0, writeBuffer.Length);

        // Close the file to prevent any corruptions
        //  file.Close();

        // string result = XElement.Parse(Encoding.ASCII.GetString(streamer.GetBuffer()).Replace("\0", "")).ToString();
        //        Debug.Log("Serialized Result: " + result);
    }

    public static void SaveGameAsync(SaveData saveData)
    {
        Thread t = new Thread(() =>
        {
            /*             BinaryFormatter formatter = new BinaryFormatter();

                        FileStream stream = new FileStream(fullSavePath, FileMode.Create);

                        formatter.Serialize(stream, saveData);
                        stream.Close(); */

            FileStream file = File.Create(fullSavePath);

            DataContractSerializer bf = new DataContractSerializer(saveData.GetType());
            MemoryStream streamer = new MemoryStream();

            //Serialize the file
            bf.WriteObject(streamer, saveData);
            streamer.Seek(0, SeekOrigin.Begin);

            //bf.ReadObject()

            string b64String = EncodeTo64(System.Text.UTF8Encoding.UTF8.GetString(streamer.GetBuffer()));

            // Debug.Log(System.Text.UTF8Encoding.UTF8.GetString(streamer.GetBuffer()));
            // Debug.Log(b64String);

            byte[] writeBuffer = System.Text.UTF8Encoding.UTF8.GetBytes(b64String);

            //Save to disk
            // File.WriteAllText(fullSavePath,b64String);
            // File.WriteAllBytes(fullSavePath,writeBuffer);
            //file.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);
            file.Write(writeBuffer, 0, writeBuffer.Length);
        });

        t.Start();

    }

    public static SaveData LoadGame()
    {
        /*         if (File.Exists(fullSavePath))
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
                } */

        if (File.Exists(fullSavePath))
        {
            // BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(fullSavePath, FileMode.Open);

            MemoryStream streamer = new MemoryStream((int)file.Length);

            // streamer.

            byte[] bytes = new byte[file.Length];

            file.Read(bytes, 0, (int)file.Length);

            //string b64String= DecodeFrom64(Convert.ToBase64String(bytes));
            string str = System.Text.UTF8Encoding.UTF8.GetString(bytes);
//            Debug.Log("str= " + str);

            string fromB64String = DecodeFrom64(str);
           // Debug.Log("fromB64String=" + fromB64String);
            byte[] readBytes = System.Text.UTF8Encoding.UTF8.GetBytes(fromB64String);

            // byte[] readBytes=DecodeFrom64(bytes);
            streamer.Write(readBytes, 0, readBytes.Length);
            //streamer.Write(bytes, 0, bytes.Length);
            DataContractSerializer bf = new DataContractSerializer(typeof(SaveData));

            streamer.Seek(0, SeekOrigin.Begin);

            //Debug.Log(streamer.Length);

            // bf.ReadObject();

            SaveData saveData = (SaveData)bf.ReadObject(streamer);

            file.Close();

            return saveData;
        }
        else
        {
            Debug.LogError("Save file not found in " + fullSavePath);

            return null;
        }

    }

    static public string EncodeTo64(string toEncode)
    {
        byte[] toEncodeAsBytes
              = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
        string returnValue
              = System.Convert.ToBase64String(toEncodeAsBytes);
        return returnValue;
    }



    static public string DecodeFrom64(string encodedData)
    {
        byte[] encodedDataAsBytes
            = System.Convert.FromBase64String(encodedData);
        string returnValue =
           System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
        return returnValue;
    }

    static public byte[] EncodeTo64(byte[] byteStringToEncode)
    {
        /*         byte[] toEncodeAsBytes
                      = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode); */
        string b64String
              = System.Convert.ToBase64String(byteStringToEncode);
        Debug.Log(b64String);
        byte[] returnValue = System.Text.ASCIIEncoding.ASCII.GetBytes(b64String);
        //byte[] returnValue = System.Convert.FromBase64String(b64String);
        return returnValue;
    }

    static public byte[] DecodeFrom64(byte[] byteStringToDecode)
    {
        string b64String = System.Convert.ToBase64String(byteStringToDecode);
        byte[] data = Convert.FromBase64String(b64String);
        string decodedString = System.Text.ASCIIEncoding.ASCII.GetString(data);
        Debug.Log(decodedString);
        byte[] returnValue = System.Text.ASCIIEncoding.ASCII.GetBytes(decodedString);

        return returnValue;
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
