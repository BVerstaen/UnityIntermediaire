using UnityEditor;
using UnityEngine;

namespace GabinBaptisteEnguerrandProject.Scripts
{
    [CustomEditor(typeof(SkillTree))]
    public class SkillTreeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var skillTree = (SkillTree)target;

            if (GUILayout.Button("Generate Skill Tree"))
            {
                //skillTree.AddLeaf();
            }
            if (GUILayout.Button("Remove Leaf"))
            {
                skillTree.ClearLeaf();
            }
        }
    }
}