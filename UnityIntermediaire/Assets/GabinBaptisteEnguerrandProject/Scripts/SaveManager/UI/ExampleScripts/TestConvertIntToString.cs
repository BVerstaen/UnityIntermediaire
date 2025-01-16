using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestConvertIntToString : MonoBehaviour
{
    [SerializeField] Text _text;

    public void ConvertIntToString(float value)
    {
        _text.text = value.ToString();
    }
}
