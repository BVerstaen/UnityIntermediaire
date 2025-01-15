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

                //Create Instance reference
                SaveSettings.Instance = settings;

                AssetDatabase.SaveAssets();
            }
        }
        return settings;
    }

    public override void OnGUI(string searchContext)
    {
        settings = GetOrCreateSettings();

        //Create Editor Settings
        EditorGUILayout.LabelField("Save Settings", EditorStyles.boldLabel);
        settings.FileFormat = (FileFormats)EditorGUILayout.EnumPopup("File format : ", settings.FileFormat);
        
        //Show binary extension if binary format is selected
        if(settings.FileFormat == FileFormats.BINARY)
            settings.FileFormatExtension = EditorGUILayout.TextField("File Extension :", settings.FileFormatExtension);
        
        settings.FolderName = EditorGUILayout.TextField("Folder Name", settings.FolderName);

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