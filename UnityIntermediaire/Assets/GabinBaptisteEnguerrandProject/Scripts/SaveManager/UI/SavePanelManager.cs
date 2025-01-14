using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePanelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject SavePanelPrefab;

    string _selectedFileName;



    public void SelectSaveFile(string newSaveFileName)
    {
        _selectedFileName = newSaveFileName;
    }

    //Buttons functions
    public void CreateSave()
    {

    }

    public void LoadSave()
    {

    }

    public void EraseSave()
    {
        if (_selectedFileName != null)
        {
            SaveManager.DeleteSave(_selectedFileName);
        }
    }
}
