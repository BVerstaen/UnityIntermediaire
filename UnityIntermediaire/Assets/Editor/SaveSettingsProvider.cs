using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static SaveManager;

public class SaveSettingsProvider : SettingsProvider
{
    private const string Path = "Project/Save Settings";
    private static SaveSettings settings;

    public SaveSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }
    
    public static SaveSettings GetOrCreateSettings()
    {
        //Find if folder exist
        string folderPath = Application.dataPath + "/Resources/SaveManager";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }


        //Get current save settings if not already
        if (settings == null)
        {
            settings = AssetDatabase.LoadAssetAtPath<SaveSettings>("Assets/Resources/SaveManager/DefaultSaveSettings.asset");
            if (settings == null)
            {
                //Create Save settings if doesn't exists
                settings = ScriptableObject.CreateInstance<SaveSettings>();
                AssetDatabase.CreateAsset(settings, "Assets/Resources/SaveManager/DefaultSaveSettings.asset");

                AssetDatabase.SaveAssets();
            }
        }
        return settings;
    }

    public override void OnGUI(string searchContext)
    {
        settings = GetOrCreateSettings();

        //Delete everything in save folder
        if (GUILayout.Button("Erase All save files"))
        {
            string SaveFolder = Application.persistentDataPath + "/" + settings.FolderName;
            string ProfileFolder = Application.persistentDataPath + "/" + settings.ProfileFolderName;
            
            if (Directory.Exists(SaveFolder))
                Directory.Delete(SaveFolder, true);

            if (Directory.Exists(ProfileFolder))
                Directory.Delete(ProfileFolder, true);
        }

        //Create Editor Settings
        EditorGUILayout.LabelField("Save Settings", EditorStyles.boldLabel);
        settings.FileFormat = (FileFormats)EditorGUILayout.EnumPopup("File format : ", settings.FileFormat);

        //Show binary extension if binary format is selected
        if(settings.FileFormat == FileFormats.BINARY)
            settings.FileFormatExtension = EditorGUILayout.TextField("File extension :", settings.FileFormatExtension);
        
        //Don't show default folder name if using profiles is selected
        if (!settings.UseProfiles)
            settings.FolderName = EditorGUILayout.TextField("Folder name", settings.FolderName);
        
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Profile Settings", EditorStyles.boldLabel);
        
        settings.UseProfiles = EditorGUILayout.ToggleLeft("Use profiles", settings.UseProfiles);
        //Show maximum number of profiles if use profiles
        if (settings.UseProfiles)
        {
            settings.MaximumNumberOfProfiles = EditorGUILayout.IntField("Maximum number of profiles", settings.MaximumNumberOfProfiles);
            settings.ProfileFolderName = EditorGUILayout.TextField("Profile folder name :", settings.ProfileFolderName);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(settings);
        }
    }

    [SettingsProvider]
    public static SettingsProvider CreateCustomSettingsProvider()
    {
        return new SaveSettingsProvider(Path, SettingsScope.Project);
    }
}