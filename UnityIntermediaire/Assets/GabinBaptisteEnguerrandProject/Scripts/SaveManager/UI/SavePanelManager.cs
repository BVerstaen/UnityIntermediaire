using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePanelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject SavePanelPrefab;
    [SerializeField] InputField SaveNameField;

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
        if(_selectedFileName != null)
        {
            object LoadedSave = SaveManager.LoadData(_selectedFileName);
            Debug.Log(LoadedSave);
        }
    }

    public void EraseSave()
    {
        if (_selectedFileName != null)
        {
            SaveManager.DeleteSave(_selectedFileName);
        }
    }
}
