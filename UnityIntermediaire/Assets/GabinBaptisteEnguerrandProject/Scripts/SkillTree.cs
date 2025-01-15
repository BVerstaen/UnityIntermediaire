using System.Collections.Generic;
using UnityEngine;

namespace GabinBaptisteEnguerrandProject.Scripts
{
    public class SkillTree : MonoBehaviour
    {
        public List<Leaf> _leaves;
        public List<Leaf> InstantiateLeaves;
        private Leaf leaf;
        public string nameText = "";
        public Vector2 position;
        public GameObject leafPrefab;
        public int price;
        public int number;
        public bool isLocked;
        private int i = 0;

        private void Start()
        {
            Debug.Log(_leaves.Count);
            leaf = leafPrefab.GetComponent<Leaf>();
            _leaves = new List<Leaf>();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public Leaf AddLeaf(string name, int price, bool isLocked, Vector2 position, int number, Leaf previousLeaf)
        {
            leaf._position = position;
            leaf._number = i++;
            leaf._name = name;
            leaf.name = leaf._name;
            leaf._price = price;
            leaf._isLocked = isLocked;
            if (leaf._number > 0)
                leaf._previousLeaf = InstantiateLeaves[leaf._number - 1];
            _leaves.Add(leaf);
            Debug.Log(_leaves.Count);
            return SpawnTree();
        }

        public Leaf SpawnTree()
        {
            InstantiateLeaves.Add(Instantiate(_leaves[leaf._number], _leaves[leaf._number]._position,
                Quaternion.identity));
            return InstantiateLeaves[InstantiateLeaves.Count - 1];
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void UnlockLeaf(int number)
        {
            if (!InstantiateLeaves[number - 1]._isLocked)
            {
                InstantiateLeaves[number]._isLocked = false;
            }
            else
            {
                Debug.Log("the leaf number : "+InstantiateLeaves[number - 1]._number + " is locked");
            }
        }

        public void RemoveLeaf(Leaf leaf)
        {
            _leaves.Remove(leaf);
            SpawnTree();
        }

        public void ClearLeaf()
        {
            _leaves.Clear();
        }

        public void Save()
        {
            //Baptiste script
        }

        private void Update()
        {
        }
    }
}