using System;
using GabinBaptisteEnguerrandProject.Prefabs.SkillTree.V2;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GabinBaptisteEnguerrandProject.Scripts
{
    [CustomEditor(typeof(MasteryTree))]
    public class MasteryTreeEditor : Editor
    {
        private SerializedProperty BranchPrefabProperty;

        private void OnEnable()
        {
            BranchPrefabProperty = serializedObject.FindProperty("BranchPrefab");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(BranchPrefabProperty, new GUIContent("Branch Prefab"));

            MasteryTree masteryTree = (MasteryTree)target;
            if (GUILayout.Button("Add branch"))
            {
                masteryTree.AddBranch();
            }

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(masteryTree);
            }
        }
    }

    [CustomEditor(typeof(Branch))]
    public class BranchEditor : Editor
    {
        private SerializedProperty LeafPrefabProperty;

        private void OnEnable()
        {
            LeafPrefabProperty = serializedObject.FindProperty("LeafPrefab");
        }

        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(LeafPrefabProperty, new GUIContent("LeafPrefab"));
            Branch branch = (Branch)target;
            branch.position = EditorGUILayout.Vector2Field("Leaf position", branch.position);
            branch.price = EditorGUILayout.IntField("Unlock Price", branch.price);
            branch.isLocked = EditorGUILayout.Toggle("Is locked", branch.isLocked);
            if (GUILayout.Button("Add leaf"))
            {
                branch.SetupLeaf(branch.position, branch.price, branch.isLocked);
                branch.AddLeaf();
            }

            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(branch);
            }
        }
    }

    [CustomEditor(typeof(Leaves))]
    public class LeavesEditor : Editor
    {
        private SerializedProperty BranchPrefabProperty;

        private void OnEnable()
        {
            BranchPrefabProperty = serializedObject.FindProperty("BranchPrefab");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(BranchPrefabProperty, new GUIContent("BranchPrefab"));
            Leaves leaf = (Leaves)target;

            if (GUILayout.Button("Add branch"))
            {
                leaf.AddBranch();
            }

            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(leaf);
            }
        }
    }
}