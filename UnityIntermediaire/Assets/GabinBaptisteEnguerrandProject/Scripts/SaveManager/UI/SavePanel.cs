using System.Collections;
using System.Collections.Generic;
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

    [Header("Custom UI")]
    [SerializeField] bool _showNameText = true;
    [SerializeField] bool _showDateText = true;
    [SerializeField] bool _showImage = true;
    [SerializeField] bool _showDeleteButton = true;

    [SerializeField] List<GameObject> _objectsActivatedWhenSelected;

    private string _saveName;

    public string SaveName { get => _saveName; private set => _saveName = value; }

    private void Awake()
    {
        //Show / Hide UI depending on choice
        if (_saveNameText != null)
            _saveNameText.enabled = _showNameText;

        if(_dateText != null)
            _dateText.enabled = _showDateText;

        if(_saveImg != null)
            _saveImg.enabled = _showImage;

        if(_deleteButton != null)
            _deleteButton.enabled = _showDeleteButton;
    }

    public void LoadDataFromSaveFile<T>(SaveFileData<T> saveFile)
    {
        if(_saveNameText != null)
            _saveNameText.text = saveFile.FileName;

        _saveName = saveFile.FileName;

        if(_dateText != null)
            _dateText.text = saveFile.FileDate;

        if (saveFile.FileImage != string.Empty && _saveImg != null)
        {
            _saveImg.sprite = Resources.Load<Sprite>("SaveManager\\" + saveFile.FileImage);
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
