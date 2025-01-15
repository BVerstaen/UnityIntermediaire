using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static SaveManager;

public static class SaveSettingsManager
{
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
        if (SaveSettings.Instance == null)
        {
            SaveSettings currentSaveSettings = AssetDatabase.LoadAssetAtPath<SaveSettings>("Assets/GabinBaptisteEnguerrandProject/Scripts/SaveManager/DefaultSaveSettings.asset");
            SaveSettings.Instance = currentSaveSettings;
        }
        return SaveSettings.Instance;
    }
}