using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SavePanelManager : MonoBehaviour
{
    public enum PanelImageType
    {
        None = 0,
        SimpleImage = 1,
        RandomImage = 2,
        Screenshot = 3
    }
    public enum ScrollRectDirection
    {
        Horizontal,
        InvertedHorizontal,
        Vertical,
        InvertedVertical
    }


    [Header("References")]
    [SerializeField] GameObject _savePanelPrefab;

    [Header("Save parameters")]
    [SerializeField] int _maxNumberOfSaves;
    [SerializeField] PanelImageType _panelImage;
    [SerializeField] Sprite _defaultPanelImage;
    [SerializeField] List<Sprite> _listOfPanelImages;


    [Header("Panel placments parameters")]
    [SerializeField] Transform _savePanelFirstPosition;
    [Space(5)]
    [SerializeField] float _spaceBetweenTwoSavePanels;
    [SerializeField] ScrollRectDirection _panelScrollDirection;
    private ScrollRect _panelScrollRect;

    [Header("Input save name")]
    [SerializeField] InputField _saveNameField;
    [SerializeField] string _defaultSaveName;
    [SerializeField] bool _shouldSaveNameAutoIncrement;

    [Header("Unity Events")]
    [SerializeField] UnityEvent _onMaxNumberOfSavesReached;

    

    //Panel private fields
    List<SavePanel> _savePanels;
    SavePanel _selectedPanel;
    Vector2 _newPanelPosition;

    private void OnValidate()
    {
        if(_maxNumberOfSaves <= 0)
            _maxNumberOfSaves = 1;
    }

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
        List<SaveManager.SaveFileData<string>> saveFiles = SaveManager.GetEverySaveFile<string>();

        //Check if there's more save files than max number
        if(saveFiles.Count > _maxNumberOfSaves)
        {
            Debug.LogWarning("There's more save files than maximumNumberOfSaves !");
        }

        //Create corresponding save panels
        foreach (var file in saveFiles) 
        {
            if(file == null) continue;

            GameObject newPanel = Instantiate(_savePanelPrefab, gameObject.transform);
            //Set new position
            newPanel.transform.localPosition = _newPanelPosition;
            switch (_panelScrollDirection)
            {
                case ScrollRectDirection.Horizontal:
                    _newPanelPosition = new Vector2(_newPanelPosition.x + _spaceBetweenTwoSavePanels, _newPanelPosition.y);
                    break;

                case ScrollRectDirection.InvertedHorizontal:
                    _newPanelPosition = new Vector2(_newPanelPosition.x - _spaceBetweenTwoSavePanels, _newPanelPosition.y);
                    break;

                case ScrollRectDirection.Vertical:
                    _newPanelPosition = new Vector2(_newPanelPosition.x, _newPanelPosition.y - _spaceBetweenTwoSavePanels);
                    break;

                case ScrollRectDirection.InvertedVertical:
                    _newPanelPosition = new Vector2(_newPanelPosition.x, _newPanelPosition.y + _spaceBetweenTwoSavePanels);
                    break;
            }
            

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

        //Set panel scroll in function of panel scroll direction
        _panelScrollRect = GetComponent<ScrollRect>();
        if (_panelScrollRect != null)
        {
            switch (_panelScrollDirection)
            {
                case ScrollRectDirection.Horizontal:
                case ScrollRectDirection.InvertedHorizontal:
                    _panelScrollRect.horizontal = true;
                    _panelScrollRect.vertical = false;
                    break;

                case ScrollRectDirection.Vertical:
                case ScrollRectDirection.InvertedVertical:
                    _panelScrollRect.horizontal = false;
                    _panelScrollRect.vertical = true;
                    break;
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
    public void CreateSaveFromComponent(Component _saveObject)
    {
        //Change save name if use save name field
        string saveName = _defaultSaveName + (_shouldSaveNameAutoIncrement ? _savePanels.Count : "");
        if(_saveNameField != null)
            saveName = _saveNameField.text;


        //Check if reach maximum number of saves files and if creating new file
        if(_maxNumberOfSaves <= _savePanels.Count && !DoesSaveAlreadyExist(saveName))
        {
            Debug.LogWarning("Maximum number of saves reached !");
            return;
        }

        
        string _savedata = JsonUtility.ToJson(_saveObject);

        Texture2D savePanelImage = null;
        bool takeScreenShot = false;
        switch (_panelImage)
        {
            case PanelImageType.None:
                break;
            
            case PanelImageType.SimpleImage:
                savePanelImage = _defaultPanelImage.texture;
                break;

            case PanelImageType.RandomImage:
                savePanelImage = _listOfPanelImages[UnityEngine.Random.Range(0, _listOfPanelImages.Count)].texture;
                break;

            case PanelImageType.Screenshot:
                takeScreenShot = true;
                break;
                
        }

        SaveManager.SaveData(_savedata, saveName, savePanelImage, takeScreenShot);

        RefreshAndCreateSavePanels();
    }
    
    public void LoadSave<T>(object ObjectToSaveTo)
    {
        if (_selectedPanel == null) throw new Exception("Missing selected Panel");

        string _loadedJson = SaveManager.LoadData<string>(_selectedPanel.SaveName);
        JsonUtility.FromJsonOverwrite(_loadedJson, ObjectToSaveTo);
    }

    public void EraseSave()
    {
        if (_selectedPanel != null)
        {
            SaveManager.DeleteSave(_selectedPanel.SaveName);
        }

        RefreshAndCreateSavePanels();
    }

    private bool DoesSaveAlreadyExist(string saveName)
    {
        string path = Application.persistentDataPath + "/" + SaveSettingsManager.GetFolderName() + "/" + saveName + "." + SaveSettingsManager.GetFileFormatExtension();

        return File.Exists(path);
    }
}
