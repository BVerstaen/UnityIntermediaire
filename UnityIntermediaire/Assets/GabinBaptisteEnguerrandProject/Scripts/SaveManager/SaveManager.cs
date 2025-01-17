using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    public enum FileFormats { JSON, BINARY }

    [System.Serializable]
    public class SaveFileData<T>
    {
        public string FileName;
        public bool hasCorrespondingImage;
        public string FileDate;

        public T Data;

        public SaveFileData(T newData, string filename, Texture2D fileImage)
        {
            FileName = filename;
            hasCorrespondingImage = fileImage != null;
            DateTime dt = DateTime.Now;
            FileDate = dt.ToString("dd/MM/yyyy - HH:mm:ss");

            Data = newData;
        }
    }

    [System.Serializable]
    public class SaveWrapper<T>
    {
        public List<T> Items;
    }

    private static string GetSaveFilePath(string saveName, bool withExtension = true)
    {
        return Application.persistentDataPath + "/" + SaveSettingsManager.GetFolderName() + "/" + saveName + (withExtension ? "." + SaveSettingsManager.GetFileFormatExtension() : "");
    }

    //Save data functions

    public static void SaveData<T>(T dataToSave, string saveName, Texture2D fileImage = null, bool takeScreenShot = false)
    {
        //Create save file data & get save path
        string path = GetSaveFilePath(saveName);
        SaveFileData<T> SaveFile = new SaveFileData<T>(dataToSave, saveName, fileImage);

        //Notify that corresponding image exist if take screenshot instead
        if (fileImage == null)
            SaveFile.hasCorrespondingImage = takeScreenShot;

        //If using profiles, then check if there's a valid profile
        if (SaveSettingsManager.UseProfiles())
        {
            if (SaveSettingsManager.GetFolderName() == "")
            {
                Debug.LogWarning("No profile selected !");
                return;
            }
        }

        //Create save foldier if doesn't exist
        string directoryPath = Application.persistentDataPath + "/" + SaveSettingsManager.GetFolderName();
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

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

        //Delete old corresponding image if already exist
        string imgPath = Application.persistentDataPath + "/" + SaveSettingsManager.GetFolderName() + "/" + saveName + ".png";
        DeleteCorrespondingImage(saveName);

        //Take and save screenshot if possible
        if (takeScreenShot)
        {
            ScreenCapture.CaptureScreenshot(imgPath);
        }
        //Save File Image if exist
        else if (fileImage != null)
        {
            if (!File.Exists(imgPath))
            {
                Texture2D texture = CreateReadableTexture(fileImage);

                if (texture.isReadable)
                {
                    byte[] fileToBytes = texture.EncodeToPNG();
                    File.WriteAllBytes(imgPath, fileToBytes);
                }
                else
                {
                    Debug.LogError(fileImage + " doesn't have his \"Read/Write\" bool set to true, therefore no fileImage can be created.");
                }
            }
        }



        Texture2D CreateReadableTexture(Texture2D source)
        {
            Texture2D newTexture = new Texture2D(source.width, source.height);

            //Copy pixels from source texture to temporary "renderTex" texture2D
            RenderTexture renderTex = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear
            );
            Graphics.Blit(source, renderTex);

            //"paste" them into newtexture
            newTexture.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            newTexture.Apply();

            //free memory allocated to renderTex
            RenderTexture.ReleaseTemporary(renderTex);

            return newTexture;
        }

    }

    public static void SaveListOfSerializableClass<T>(List<T> dataToSave, string saveName, Texture2D fileImage = null, bool takeScreenShot = false)
    {
        string json = JsonUtility.ToJson(new SaveWrapper<T> { Items = dataToSave}, true);
        SaveManager.SaveData<string>(json, saveName, null, true);
    }

    //Fetch data functions

    public static List<SaveFileData<T>> GetEverySaveFile<T>()
    {
        List<SaveFileData<T>> SaveFilesList = new List<SaveFileData<T>>();
        string folderPath = Application.persistentDataPath + "/" + SaveSettingsManager.GetFolderName();

        if (Directory.Exists(folderPath))
        {
            //Filter files by extension name
            string[] filesFound = Directory.GetFiles(folderPath, "*." + SaveSettingsManager.GetFileFormatExtension());
            foreach (string file in filesFound)
            {
                //Get file names and get save file data 
                string fileName = Path.GetFileName(file);
                SaveFilesList.Add(GetSaveFileData<T>(fileName));
            }
        }
        else
        {
            Debug.LogWarning("Can't find folder in " + folderPath + " creating a new one...");
            Directory.CreateDirectory(folderPath);
        }
        
        return SaveFilesList;
    }

    public static SaveFileData<T> GetSaveFileData<T>(string saveName)
    {
        string path = GetSaveFilePath(saveName, false);

        if (File.Exists(path))
        {
            SaveFileData<T> dataToLoad = null;

            switch (SaveSettingsManager.GetFileFormat())
            {
                //Load SaveFileData from .JSON
                case FileFormats.JSON:
                    string SaveDataJSON = File.ReadAllText(path);
                    dataToLoad = JsonUtility.FromJson<SaveFileData<T>>(SaveDataJSON);
                    break;

                //Load SaveFileData from Binary file
                case FileFormats.BINARY:
                    //Load data & get save files
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream stream = new FileStream(path, FileMode.Open);

                    dataToLoad = formatter.Deserialize(stream) as SaveFileData<T>;
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

    //Load functions

    public static T LoadData<T>(string saveName)
    {
        SaveFileData<T> dataToLoad = GetSaveFileData<T>(saveName + "." + SaveSettingsManager.GetFileFormatExtension());
        
        if (dataToLoad == null)
        {
            Debug.LogError("Can't load save file !");
        }

        return dataToLoad.Data;
    }

    public static List<T> LoadListOfSerializableClass<T>(string saveName)
    {
        string ListInJson = SaveManager.LoadData<string>(saveName);
        SaveWrapper<T> LoadedData = new SaveWrapper<T>();
        JsonUtility.FromJsonOverwrite(ListInJson, LoadedData);

        return LoadedData.Items;
    }

    //Delete functions

    public static void DeleteSave(string saveName)
    {
        string path = GetSaveFilePath(saveName);
        if (File.Exists(path))
            File.Delete(path);
        else
            Debug.LogError("Save file can't be found in " + path);

        DeleteCorrespondingImage(saveName);
    }

    private static void DeleteCorrespondingImage(string saveName)
    {
        string correspondingImagePath = Application.persistentDataPath + "/" + SaveSettingsManager.GetFolderName() + "/" + saveName + ".png";
        if (File.Exists(correspondingImagePath))
            File.Delete(correspondingImagePath);
    }
}