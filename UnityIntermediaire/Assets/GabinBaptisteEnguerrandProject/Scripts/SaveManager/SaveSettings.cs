using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SaveManager;

public class SaveSettings : ScriptableObject
{
    public FileFormats FileFormat = FileFormats.BINARY;
    public string FileFormatExtension = "bin";
    public string FolderName = "Saves";

    public bool UseProfiles;
    public int MaximumNumberOfProfiles;
    public string ProfileFolderName = "Profiles";
    
    public string SelectedFolderName = "";
}

