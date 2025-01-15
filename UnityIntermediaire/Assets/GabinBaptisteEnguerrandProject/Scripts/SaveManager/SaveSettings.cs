using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SaveManager;

public class SaveSettings : ScriptableObject
{
    public static SaveSettings Instance;

    public FileFormats FileFormat = FileFormats.BINARY;
    public string FileFormatExtension = "bin";
    public string FolderName = "Saves";
}

