using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSaveScript : MonoBehaviour, ISaveable
{
    public float FloatA;
    public float FloatB;
    public int IntA;
    public string InputText;

    [SerializeField] SavePanelManager _savePanelManager;

    public void UpdateFloatA(float newValue) => FloatA = newValue;
    public void UpdateFloatB(float newValue) => FloatB = newValue;
    public void UpdateIntA(float newValue) => IntA = (int)newValue;
    public void UpdateInputText(string newValue) => InputText = newValue;

    public void LoadValues()
    {
        TestSaveScript newTestSave = _savePanelManager.LoadSave<TestSaveScript>();

        FloatA = newTestSave.FloatA;
        FloatB = newTestSave.FloatB;
        IntA = newTestSave.IntA;
        InputText = newTestSave.InputText;
    }

    public void SaveValues()
    {
        _savePanelManager.CreateSave(this);
    }
}