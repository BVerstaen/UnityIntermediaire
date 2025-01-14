using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public static class SaveManager
{
    public enum FileFormats { JSON, BINARY }

    public class SaveFileData
    {
        public string FileName;
        public Image FileImage;
        public string FileDate;

        public object Data;

        public SaveFileData(object newData, string filename, Image fileImage)
        {
            FileName = filename;
            FileImage = fileImage;

            DateTime dt = DateTime.Now;
            FileDate = dt.ToString("dd-MM-yyyy HH:mm:ss");


            Data = newData;
        }
    }

    public static void SaveProgress(object dataToSave, string saveName = "Save", string fileName = "Save", Image FileImage = null)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "Saves" + "/" + saveName + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveFileData SaveFile = new SaveFileData(dataToSave, fileName, FileImage);

        formatter.Serialize(stream, SaveFile);

        stream.Close();
        
        Debug.LogFormat("Save Complete !");
    }

    public static List<SaveFileData> GetEverySaveFileData()
    {
        return new List<SaveFileData>();
    }

    public static SaveFileData GetSaveFileData(string saveName)
    {
        string path = Application.persistentDataPath + "/" + "Saves" + "/" + saveName + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveFileData dataToLoad = formatter.Deserialize(stream) as SaveFileData;
            stream.Close();

            Debug.LogFormat("Load Complete !");
            return dataToLoad;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static object LoadData()
    {
        string path = Application.persistentDataPath + "/SaveData.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveFileData dataToLoad = formatter.Deserialize(stream) as SaveFileData;
            stream.Close();

            Debug.LogFormat("Load Complete !");
            return dataToLoad.Data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }

    }

    public static void DeleteSave(string fileName = "Save")
    {
        string path = Application.persistentDataPath + "/SaveData.save";

    }
}