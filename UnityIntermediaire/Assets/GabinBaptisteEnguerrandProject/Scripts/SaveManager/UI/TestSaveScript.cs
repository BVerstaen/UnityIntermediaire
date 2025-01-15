using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSaveScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SavePanelManager _savePanelManager;
    [SerializeField] private TestSaveContener _saveContenerComponent;
    [Space(2)]
    [SerializeField] Slider _FloatA;
    [SerializeField] Slider _FloatB;
    [SerializeField] Slider _IntA;
    [SerializeField] InputField _InputField;
    private void Awake()
    {
        _saveContenerComponent.GetComponent<TestSaveContener>();
    }

    public void LoadValues()
    {
        _savePanelManager.LoadSave<TestSaveScript>(_saveContenerComponent);

        _FloatA.value = _saveContenerComponent.FloatA;
        _FloatB.value = _saveContenerComponent.FloatB;
        _IntA.value = _saveContenerComponent.IntA;
        _InputField.text = _saveContenerComponent.InputText;
    }

    public void SaveValues()
    {
        _savePanelManager.CreateSaveFromComponent(_saveContenerComponent);
    }
}