using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static SaveManager;

public class SaveSettings : ScriptableObject
{
    public static SaveSettings Instance;

    public FileFormats FileFormat = FileFormats.BINARY;
    public string FileFormatExtension = "bin";
    public string FolderName = "Saves";
}

public static class SaveSettingsManager
{
    public static string GetFolderName()
    {
        return SaveSettings.Instance.FolderName;
    }

    public static FileFormats GetFileFormat()
    {
        return SaveSettings.Instance.FileFormat;
    }

    public static string GetFileFormatExtension()
    {
        switch (GetFileFormat())
        {
            case FileFormats.BINARY:
                return SaveSettings.Instance.FileFormatExtension;

            case FileFormats.JSON:
                return "Json";
        }

        return "";
    }
}