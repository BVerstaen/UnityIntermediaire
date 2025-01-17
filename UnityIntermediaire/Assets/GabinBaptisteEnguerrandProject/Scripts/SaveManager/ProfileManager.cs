using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ProfileManager
{
    public static void CreateProfile(string profileName)
    {
        string path = Application.persistentDataPath + "/" + SaveSettingsManager.GetProfileFolderName() + "/" + profileName;

        if (Directory.Exists(path))
        {
            Debug.LogError("Profile " + profileName + " already exist");
            return;
        }

        if (SaveSettingsManager.GetMaximumNumberOfProfiles() <= Directory.GetDirectories(Application.persistentDataPath + "/" + SaveSettingsManager.GetProfileFolderName()).Length)
        {
            Debug.LogWarning("Maximum number of profiles reached !");
            return;
        }

        Directory.CreateDirectory(path);
    }

    public static void EraseProfile(string profileName)
    {
        string path = Application.persistentDataPath + "/" + profileName + "/";

        if (!Directory.Exists(path))
        {
            Debug.LogError("Can't find profile : " + profileName);
            return;
        }

        Directory.Delete(path, true);

        //Change folder name to unknwown value
        SaveSettingsManager.ChangeProfileFolderName("");
    }

    public static void ChangeProfile(string newProfileName)
    {
        string path = Application.persistentDataPath + "/" + SaveSettingsManager.GetProfileFolderName() + "/" + newProfileName;

        if (!Directory.Exists(path))
        {
            Debug.LogError("Can't find profile : " + newProfileName);
            return;
        }
        SaveSettingsManager.ChangeProfileFolderName(newProfileName);
    }

    public static string GetCurrentProfile()
    {
        return SaveSettingsManager.GetFolderName();
    }

    public static string[] GetEveryProfiles()
    {
        return Directory.GetDirectories(Application.persistentDataPath + "/" + SaveSettingsManager.GetProfileFolderName());
    }
}
