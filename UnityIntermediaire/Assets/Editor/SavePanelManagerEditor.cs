using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SavePanelManager))]
public class SavePanelManagerEditor : Editor
{
    SerializedProperty _savePanelPrefab;
    SerializedProperty _gameObjectToSave;
    
    SerializedProperty _maxNumberOfSaves;
    SerializedProperty _panelImage;
    SerializedProperty _defaultPanelImage;
    SerializedProperty _listOfPanelImages;

    SerializedProperty _savePanelFirstPosition;
    SerializedProperty _spaceBetweenTwoSavePanels;
    SerializedProperty _panelScrollDirection;

    SerializedProperty _saveNameField;
    SerializedProperty _defaultSaveName;
    SerializedProperty _shouldSaveNameAutoIncrement;

    SerializedProperty _onMaxNumberOfSavesReached;

    private void OnEnable()
    {

        _savePanelPrefab = serializedObject.FindProperty("_savePanelPrefab");
        _gameObjectToSave = serializedObject.FindProperty("_gameObjectToSave");
        
        _maxNumberOfSaves = serializedObject.FindProperty("_maxNumberOfSaves");
        _panelImage = serializedObject.FindProperty("_panelImage");
        _defaultPanelImage = serializedObject.FindProperty("_defaultPanelImage");
        _listOfPanelImages = serializedObject.FindProperty("_listOfPanelImages");

        _savePanelFirstPosition = serializedObject.FindProperty("_savePanelFirstPosition");
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
        EditorGUILayout.Space(10);


        GUILayout.Box("", GUILayout.Height(5), GUILayout.ExpandWidth(true)); // Ligne horizontale
        //Get selected Gameobject and change color label
        GameObject SelectedGameObject = _gameObjectToSave.objectReferenceValue as GameObject;
        if(SelectedGameObject != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;
        EditorGUILayout.PropertyField(_gameObjectToSave);
        GUI.color = Color.white;
        EditorGUILayout.Space(10);

        if(SelectedGameObject != null)
        {
            Component[] components = SelectedGameObject.GetComponents(typeof(Component));
            SavePanelManager savePanelManager = target as SavePanelManager;
            
            // Affiche tous les composants disponibles
            if (components.Length > 0)
            {
                EditorGUILayout.LabelField("Available Components:");

                foreach (Component component in components)
                {
                    if (GUILayout.Button(component.GetType().Name))
                    {
                        // Assigner le composant sélectionné
                        savePanelManager._componentToSave = component;
                        EditorUtility.SetDirty(savePanelManager);
                    }
                }
            }
            // Affiche la variable sélectionnée
            EditorGUILayout.LabelField("Selected Component:");
            EditorGUILayout.ObjectField(savePanelManager._componentToSave, typeof(Component), true);
        }

        GUILayout.Box("", GUILayout.Height(5), GUILayout.ExpandWidth(true)); // Ligne horizontale


        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(_maxNumberOfSaves);
        EditorGUILayout.PropertyField(_panelImage);
        switch (_panelImage.intValue)
        {
            case 0:
            case 3:
            default:
                break;

            case 1:
                EditorGUILayout.PropertyField(_defaultPanelImage);
                break;

            case 2:
                EditorGUILayout.PropertyField(_listOfPanelImages);
                break;
        }
        EditorGUILayout.Space(10);

        //Check if has a first PanelPosition
        var firstPanelPositionID = _savePanelFirstPosition.objectReferenceInstanceIDValue;
        if (firstPanelPositionID != 0)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;
        EditorGUILayout.PropertyField(_savePanelFirstPosition);
        GUI.color = Color.white;

        //Repercute first panel to other properties
        if (firstPanelPositionID == 0)
        {
            EditorGUILayout.LabelField("No panel first position selected ! First panel position will be at center of the screen");
        }
        EditorGUILayout.PropertyField(_spaceBetweenTwoSavePanels);
        EditorGUILayout.PropertyField(_panelScrollDirection);
        GUI.color = Color.white;


        EditorGUILayout.PropertyField(_saveNameField);
        if (_saveNameField.objectReferenceValue == null)
        {
            EditorGUILayout.PropertyField(_defaultSaveName);
            EditorGUILayout.PropertyField(_shouldSaveNameAutoIncrement);
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(_onMaxNumberOfSavesReached);

        serializedObject.ApplyModifiedProperties();
    }

    public class ComponentSelector : MonoBehaviour
    {
        public Component SelectedComponent; // La variable où sera assignée la sélection

        // Exemple : Récupère tous les composants de type MonoBehaviour attachés à cet objet
        public void FetchComponents()
        {
            AvailableComponents = GetComponents<MonoBehaviour>();
        }

        // Liste des composants disponibles
        [HideInInspector] // Cacher dans l'inspecteur, géré par l'éditeur
        public Component[] AvailableComponents;
    }
}
