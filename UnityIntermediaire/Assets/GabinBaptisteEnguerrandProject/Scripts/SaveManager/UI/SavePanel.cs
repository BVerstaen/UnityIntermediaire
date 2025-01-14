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

    [Header("Parameters")]
    [SerializeField] bool _showDeleteButton;

    private string _saveName;

    private void Awake()
    {
        _deleteButton.enabled = _showDeleteButton;
    }

    public void LoadDataFromSaveFile(SaveFileData saveFile)
    {
        _saveNameText.text = saveFile.FileName;
        _saveName = saveFile.FileName;

        _dateText.text = saveFile.FileDate;

        if(saveFile.FileImage != null)
        {
            _saveImg.sprite = saveFile.FileImage;
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
}
