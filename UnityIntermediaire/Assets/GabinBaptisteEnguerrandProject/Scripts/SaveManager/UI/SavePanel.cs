using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        if (saveFile.hasCorrespondingImage && _saveImg != null)
        {
            string path = Application.persistentDataPath + "/" + SaveSettingsManager.GetFolderName() + "/" + saveFile.FileName + ".png";
            StartCoroutine(WaitAndApplyImageFile(path));
        }
        else
        {
            _saveImg.enabled = false;
        }
    }

    //Wait until correspondingimage is loaded and apply it
    IEnumerator WaitAndApplyImageFile(string path)
    {
        bool isImgLoaded = false;

        while (!isImgLoaded) 
        {
            isImgLoaded = File.Exists(path);
            yield return null;
        }

        byte[] imgData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(imgData))
        {
            _saveImg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
            _saveImg.enabled = true;
        }
        else
        {
            Debug.LogError("Can't load image data from Texture2D");
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
