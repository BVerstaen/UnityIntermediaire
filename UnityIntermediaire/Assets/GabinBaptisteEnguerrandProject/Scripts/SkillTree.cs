using System.Collections.Generic;
using UnityEngine;

namespace GabinBaptisteEnguerrandProject.Scripts
{
    public class SkillTree : MonoBehaviour
    {
        public List<Leaf> _leaf;

        private void Start()
        {
            Debug.Log(_leaf.Count);
            _leaf = new List<Leaf>();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void AddLeaf(string name, int price, bool isLocked, Vector3 position, int number)
        {
            _leaf.Add(new Leaf(name, price, isLocked, position, number));
            Debug.Log(_leaf.Count);
        }

        public void RemoveLeaf(Leaf leaf)
        {
            _leaf.Remove(leaf);
        }

        public void ClearLeaf()
        {
            _leaf.Clear();
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