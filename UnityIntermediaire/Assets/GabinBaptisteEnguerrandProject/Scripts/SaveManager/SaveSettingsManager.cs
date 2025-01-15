using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static SaveManager;

public static class SaveSettingsManager
{
    static SaveSettings _loadedSettings;

    public static string GetFolderName()
    {
        return GetCurrentSavesSettings().FolderName;
    }

    public static FileFormats GetFileFormat()
    {
        return GetCurrentSavesSettings().FileFormat;
    }

    public static string GetFileFormatExtension()
    {
        switch (GetFileFormat())
        {
            case FileFormats.BINARY:
                return GetCurrentSavesSettings().FileFormatExtension;

            case FileFormats.JSON:
                return "json";
        }

        return "";
    }

    private static SaveSettings GetCurrentSavesSettings()
    {
        if (_loadedSettings == null)
        {
            SaveSettings currentSaveSettings = Resources.Load("SaveManager\\DefaultSaveSettings") as SaveSettings;
            if(currentSaveSettings == null)
            {
                Debug.LogWarning("No Default Save Settings Present in Ressources/SaveManager/DefaultSaveSettings, creating one per default...");
                currentSaveSettings =  ScriptableObject.CreateInstance<SaveSettings>();
            }

            _loadedSettings = currentSaveSettings;
        }
        return _loadedSettings;
    }
}