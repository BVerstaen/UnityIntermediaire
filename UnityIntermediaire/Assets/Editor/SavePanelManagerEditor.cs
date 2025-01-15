using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting.YamlDotNet.Serialization;

[CustomEditor(typeof(SavePanelManager))]
public class SavePanelManagerEditor : Editor
{
    SerializedProperty _savePanelPrefab;
    SerializedProperty _maxNumberOfSaves;
    SerializedProperty _savePanelFirstPosition;
    SerializedProperty _savePanelImage;
    SerializedProperty _spaceBetweenTwoSavePanels;
    SerializedProperty _panelScrollDirection;

    SerializedProperty _saveNameField;
    SerializedProperty _defaultSaveName;
    SerializedProperty _shouldSaveNameAutoIncrement;

    SerializedProperty _onMaxNumberOfSavesReached;


    private void OnEnable()
    {
        _savePanelPrefab = serializedObject.FindProperty("_savePanelPrefab");
        _maxNumberOfSaves = serializedObject.FindProperty("_maxNumberOfSaves");
        _savePanelFirstPosition = serializedObject.FindProperty("_savePanelFirstPosition");
        _savePanelImage = serializedObject.FindProperty("_savePanelImage");
        _spaceBetweenTwoSavePanels = serializedObject.FindProperty("_spaceBetweenTwoSavePanels");
        _panelScrollDirection = serializedObject.FindProperty("_panelScrollDirection");
        
        _onMaxNumberOfSavesReached = serializedObject.FindProperty("_onMaxNumberOfSavesReached");

        _saveNameField = serializedObject.FindProperty("_saveNameField");
        _defaultSaveName = serializedObject.FindProperty("_defaultSaveName");
        _shouldSaveNameAutoIncrement = serializedObject.FindProperty("_shouldSaveNameAutoIncrement");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_savePanelPrefab);
        EditorGUILayout.PropertyField(_maxNumberOfSaves);
        EditorGUILayout.PropertyField(_savePanelFirstPosition);
        EditorGUILayout.PropertyField(_savePanelImage);
        EditorGUILayout.PropertyField(_spaceBetweenTwoSavePanels);
        EditorGUILayout.PropertyField(_panelScrollDirection);

        EditorGUILayout.PropertyField(_saveNameField);
        if (_saveNameField.objectReferenceValue == null)
        {
            EditorGUILayout.PropertyField(_defaultSaveName);
            EditorGUILayout.PropertyField(_shouldSaveNameAutoIncrement);
        }

        EditorGUILayout.PropertyField(_onMaxNumberOfSavesReached);

        serializedObject.ApplyModifiedProperties();
    }
}
