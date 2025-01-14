using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static SaveManager;

[CreateAssetMenu(fileName = "SaveSettings", menuName = "Settings/Save Settings")]
public class CustomSettings : ScriptableObject
{
    public FileFormats FileFormat;
    public string FolderName;
}

public class CustomSettingsProvider : SettingsProvider
{
    private const string Path = "Project/Custom Settings";
    private static CustomSettings settings;

    public CustomSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

    public static CustomSettings GetOrCreateSettings()
    {
        if (settings == null)
        {
            settings = AssetDatabase.LoadAssetAtPath<CustomSettings>("Assets/GabinBaptisteEnguerrandProject/DefaultSaveSettings.asset");
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<CustomSettings>();
                AssetDatabase.CreateAsset(settings, "Assets/GabinBaptisteEnguerrandProject/DefaultSaveSettings.asset");
                AssetDatabase.SaveAssets();
            }
        }
        return settings;
    }

    public override void OnGUI(string searchContext)
    {
        settings = GetOrCreateSettings();

        EditorGUILayout.LabelField("Save Settings", EditorStyles.boldLabel);
        settings.FileFormat = (FileFormats)EditorGUILayout.EnumPopup("File format : ", settings.FileFormat);
        settings.FolderName = EditorGUILayout.TextField("Folder Name", settings.FolderName);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(settings);
        }
    }

    [SettingsProvider]
    public static SettingsProvider CreateCustomSettingsProvider()
    {
        return new CustomSettingsProvider(Path, SettingsScope.Project);
    }
}