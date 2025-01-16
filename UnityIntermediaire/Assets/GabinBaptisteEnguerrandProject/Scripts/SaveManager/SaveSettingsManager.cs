using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SaveManager;

public static class SaveSettingsManager
{
    static SaveSettings _loadedSettings;

    public static string GetFolderName()
    {
        SaveSettings currentSaveSettings = GetCurrentSavesSettings();
        return currentSaveSettings.UseProfiles ? currentSaveSettings.ProfileFolderName + "/" + currentSaveSettings.SelectedFolderName : GetCurrentSavesSettings().FolderName;
    }

    public static FileFormats GetFileFormat()
    {
        return  GetCurrentSavesSettings().FileFormat;
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

    public static bool UseProfiles()
    {
        return GetCurrentSavesSettings().UseProfiles;
    }

    public static int GetMaximumNumberOfProfiles()
    {
        return GetCurrentSavesSettings().MaximumNumberOfProfiles;
    }

    public static string GetProfileFolderName()
    {
        return GetCurrentSavesSettings().ProfileFolderName;
    }

    public static void ChangeProfileFolderName(string newFolderName)
    {
        SaveSettings currentSaveSettings = GetCurrentSavesSettings();
        currentSaveSettings.SelectedFolderName = newFolderName;
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