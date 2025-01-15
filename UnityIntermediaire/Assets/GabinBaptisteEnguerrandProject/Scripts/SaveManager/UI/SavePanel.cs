using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class SavePanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Text _saveNameText;
    [SerializeField] Text _dateText;
    [SerializeField] Image _saveImg;
    [SerializeField] Button _deleteButton;

    [Header("Parameters")]
    [SerializeField] bool _showDeleteButton;

    [SerializeField] List<GameObject> _objectsActivatedWhenSelected;

    private string _saveName;

    public string SaveName { get => _saveName; private set => _saveName = value; }

    private void Awake()
    {
        if(_deleteButton != null)
            _deleteButton.enabled = _showDeleteButton;
    }

    public void LoadDataFromSaveFile(SaveFileData saveFile)
    {
        if(_saveNameText != null)
            _saveNameText.text = saveFile.FileName;

        _saveName = saveFile.FileName;

        if(_dateText != null)
            _dateText.text = saveFile.FileDate;

        if (saveFile.FileImage != string.Empty && _saveImg != null)
        {

            _saveImg.sprite = Resources.Load("SaveManager\\" + saveFile.FileImage) as Sprite;
            _saveImg.enabled = true;
        }
        else
        {
            _saveImg.enabled = false;
        }
    }

    public void DeleteCorrespondingSaveFile()
    {
        SaveManager.DeleteSave(_saveName);
        Destroy(gameObject);
    }

    public void SelectedEffect(bool activate)
    {
        foreach(GameObject objectActivated in _objectsActivatedWhenSelected)
        {
            objectActivated.SetActive(activate);
        }
    }
}
