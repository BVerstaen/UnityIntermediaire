using UnityEditor;
using UnityEngine;

namespace GabinBaptisteEnguerrandProject.Scripts
{
    [CustomEditor(typeof(SkillTree))]
    public class SkillTreeEditor : Editor
    {
        [SerializeField] private GameObject leafPrefab;
        private SerializedProperty leafPrefabProperty;
        private Vector2 position = Vector2.zero;

        private void OnEnable()
        {
            leafPrefabProperty = serializedObject.FindProperty("leafPrefab");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Leaf oldLeaf = null;
            EditorGUILayout.PropertyField(leafPrefabProperty, new GUIContent("Leaf Prefab"));

            var skillTree = (SkillTree)target;
            skillTree.nameText = EditorGUILayout.TextField("Leaf name", skillTree.nameText);
            skillTree.position = EditorGUILayout.Vector2Field("Leaf position", skillTree.position);
            skillTree.price = EditorGUILayout.IntField("Unlock Price", skillTree.price);
            skillTree.number = EditorGUILayout.IntField("Number of the leaf you want test", skillTree.number);
            skillTree.isLocked = EditorGUILayout.Toggle("Is locked", skillTree.isLocked);

            if (GUILayout.Button("Add leaf"))
            {
                oldLeaf = skillTree.AddLeaf(skillTree.nameText, skillTree.price, skillTree.isLocked, skillTree.position,
                    0, oldLeaf);
            }

            if (GUILayout.Button("Remove Leaf"))
            {
                skillTree.RemoveLastLeaf();
            }

            if (GUILayout.Button("UnlockLeaf"))
            {
                skillTree.UnlockLeaf(skillTree.number,skillTree.price);
            }

            if (GUILayout.Button("Clear Leaf"))
            {
                skillTree.ClearLeaf();
            }

            GUILayout.Space(25);

            if (GUILayout.Button("Save leaves"))
            {
                skillTree.Save();
            }

            if (GUILayout.Button("Load leaves"))
            {
                skillTree.LoadTree();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}