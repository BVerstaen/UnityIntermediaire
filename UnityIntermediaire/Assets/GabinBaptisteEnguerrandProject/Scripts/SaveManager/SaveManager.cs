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

    private static string GetPath(string saveName) => Application.persistentDataPath + "/" + SaveSettingsManager.GetFolderName() + "/" + saveName + "." + SaveSettingsManager.GetFileFormatExtension();

    public static void SaveProgress(this object dataToSave, string saveName = "Save", Image FileImage = null)
    {
        //Create save file data & get save path
        string path = GetPath(saveName);
        SaveFileData SaveFile = new SaveFileData(dataToSave, saveName, FileImage);

        switch (SaveSettingsManager.GetFileFormat())
        {
            //Save to .JSON
            case FileFormats.JSON:
                string SaveDataJSON = JsonUtility.ToJson(SaveFile);
                File.WriteAllText(path, SaveDataJSON);
                break;

            //Save to Binary file
            case FileFormats.BINARY:
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Create);

                formatter.Serialize(stream, SaveFile);

                stream.Close();

                Debug.Log("Save Complete !");
                break;
        }

    }

    public static List<SaveFileData> GetEverySaveFileData()
    {
        List<SaveFileData> SaveFilesList = new List<SaveFileData>();
        string folderPath = Application.persistentDataPath + "/" + SaveSettingsManager.GetFolderName();

        if (Directory.Exists(folderPath))
        {
            //Filter files by extension name
            string[] filesFound = Directory.GetFiles(folderPath, "*." + SaveSettingsManager.GetFileFormatExtension());
            foreach (string file in filesFound)
            {
                //Get file names and get save file data
                string fileName = Path.GetFileName(file);
                SaveFilesList.Add(GetSaveFileData(fileName));
            }
        }
        else
        {
            Debug.LogError("Can't find folder in " + folderPath);
        }

        return SaveFilesList;
    }

    public static SaveFileData GetSaveFileData(string saveName = "Save")
    {
        string path = GetPath(saveName);
        if (File.Exists(path))
        {
            SaveFileData dataToLoad = null;

            switch (SaveSettingsManager.GetFileFormat())
            {
                //Load SaveFileData from .JSON
                case FileFormats.JSON:
                    string SaveDataJSON = File.ReadAllText(path);
                    dataToLoad = JsonUtility.FromJson<SaveFileData>(SaveDataJSON);
                    break;

                //Load SaveFileData from Binary file
                case FileFormats.BINARY:
                    //Load data & get save files
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream stream = new FileStream(path, FileMode.Open);

                    dataToLoad = formatter.Deserialize(stream) as SaveFileData;
                    stream.Close();

                    Debug.Log("Fetch data Complete !");
                    break;
            }

            return dataToLoad;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static object LoadData(string saveName = "Save")
    {
        SaveFileData dataToLoad = GetSaveFileData(saveName);
        if (dataToLoad != null)
            return dataToLoad.Data;
        else
        {
            Debug.LogError("Can't load save file !");
            return null;
        }

    }

    public static void DeleteSave(string saveName = "Save")
    {
        string path = GetPath(saveName);
        if (File.Exists(path))
            File.Delete(path);
        else
            Debug.LogError("Save file can't be found in " + path);
    }
}