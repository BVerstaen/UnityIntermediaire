using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SavePanelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _savePanelPrefab;
    [SerializeField] InputField _saveNameField;

    [Header("Panel Parameters")]
    [SerializeField] Transform _savePanelFirstPosition;
    [SerializeField] Sprite _savePanelImage;
    [SerializeField] float _spaceBetweenTwoSavePanels;

    List<SavePanel> _savePanels;
    SavePanel _selectedPanel;

    Vector2 _newPanelPosition;

    private void Start()
    {
        _newPanelPosition = _savePanelFirstPosition.localPosition;
        _savePanels = new List<SavePanel>();
        RefreshAndCreateSavePanels();
    }

    public void RefreshAndCreateSavePanels()
    {

        //Remove old saves
        if(_savePanels.Count > 0)
        {
            foreach (var panel in _savePanels)
            {
                if(panel != null)
                    Destroy(panel.gameObject);
            }
            _newPanelPosition = _savePanelFirstPosition.localPosition;
            _savePanels.Clear();
        }

        //Get every saves
        List<SaveManager.SaveFileData> saveFiles = SaveManager.GetEverySaveFile();

        //Create corresponding save panels
        foreach (var file in saveFiles) 
        {
            if(file == null) continue;

            GameObject newPanel = Instantiate(_savePanelPrefab, gameObject.transform);
            //Set new position
            newPanel.transform.localPosition = _newPanelPosition;
            _newPanelPosition = new Vector2(_newPanelPosition.x + _spaceBetweenTwoSavePanels, _newPanelPosition.y);

            //Init saves
            SavePanel savePanelComponent = newPanel.GetComponent<SavePanel>();
            if (savePanelComponent != null)
            {                
                _savePanels.Add(savePanelComponent);
                savePanelComponent.LoadDataFromSaveFile(file);


                //Bind to selection if click on save
                Button panelButton = newPanel.GetComponent<Button>();
                if (panelButton != null)
                {
                    panelButton.onClick.AddListener(() => SelectSaveFile(savePanelComponent));
                }
            }
        }
    }

    public void SelectSaveFile(SavePanel newSavePanel)
    {
        //DeActivate previous selection effect
        if(_selectedPanel != null)
            _selectedPanel.SelectedEffect(false);

        _selectedPanel = newSavePanel;
        _selectedPanel.SelectedEffect(true);
    }

    //Buttons functions
    public void CreateSave()
    {
        //TEMP Create false object to test saving
        string newObject = "ok";

        //Change save name if use save name field
        string saveName = "Save";
        if(_saveNameField != null)
            saveName = _saveNameField.text;

        SaveManager.SaveData(newObject, saveName, _savePanelImage);

        RefreshAndCreateSavePanels();
    }

    public void LoadSave()
    {
        if(_selectedPanel != null)
        {
            string LoadedSave = SaveManager.LoadData(_selectedPanel.SaveName);
            Debug.Log(LoadedSave);
        }

    }

    public void EraseSave()
    {
        if (_selectedPanel != null)
        {
            SaveManager.DeleteSave(_selectedPanel.SaveName);
        }

        RefreshAndCreateSavePanels();
    }
}
