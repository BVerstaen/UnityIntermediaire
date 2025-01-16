using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSaveContener : MonoBehaviour
{
    public float FloatA;
    public float FloatB;
    public int IntA;
    public string InputText;

    public void UpdateFloatA(float newValue) => FloatA = newValue;
    public void UpdateFloatB(float newValue) => FloatB = newValue;
    public void UpdateIntA(float newValue) => IntA = (int)newValue;
    public void UpdateInputText(string newValue) => InputText = newValue;
}
