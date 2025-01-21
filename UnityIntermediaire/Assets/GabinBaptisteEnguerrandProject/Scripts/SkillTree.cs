using System;
using System.Collections.Generic;
using UnityEngine;

namespace GabinBaptisteEnguerrandProject.Scripts
{
    public class SkillTree : MonoBehaviour
    {
        public List<Leaf> _leaves; //feuilles qui vont être instantier
        public List<Leaf> InstantiateLeaves; //feuilles instantier
        private Leaf leaf;
        public string nameText = "";
        public Vector2 position;
        public GameObject leafPrefab; //référence du prefab leaf
        public int price; //prix pour unlock
        public int number; //numéro de la feuille
        public bool isLocked; //lock la feuille
        private int i = 0; // itérateur

        private LineRenderer _lineRenderer;
        private void Start()
        {
            Debug.Log(_leaves.Count);
            leaf = leafPrefab.GetComponent<Leaf>();
            _lineRenderer = GetComponent<LineRenderer>();
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
           // if (leaf._number > 0)
               // leaf._previousLeaf = InstantiateLeaves[leaf._number - 1];
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
        public void UnlockLeaf(int number, int prix)
        {
            if (InstantiateLeaves[number]._number == 0)
            {
                if (prix >= InstantiateLeaves[number]._price)
                {
                    InstantiateLeaves[number]._isLocked = false;
                    return;
                }
                else
                {
                    Debug.Log("the price of the leaf is : " + InstantiateLeaves[number]._price + " you have : " +
                              prix +
                              ", you don't have enough");
                    return;
                }
            }


            if (!InstantiateLeaves[number - 1]._isLocked) //Si la feuille précédente est unlock
            {
                if (prix >= InstantiateLeaves[number]._price)
                {
                    InstantiateLeaves[number]._isLocked = false; //rendre la feuille débloquer
                }
                else
                {
                    Debug.Log("the price of the leaf is : " + InstantiateLeaves[number - 1]._price + "you have : " +
                              prix +
                              ", you don't have enough");
                }
            }
            else
            {
                Debug.Log("the leaf number : " + InstantiateLeaves[number - 1]._number + " is locked");
            }
        }

        public void RemoveLastLeaf() //Supprime la dernière feuille
        {
            _leaves.RemoveAt(_leaves.Count - 1);
            Destroy(InstantiateLeaves[^1].gameObject);
            InstantiateLeaves.RemoveAt(InstantiateLeaves.Count - 1);
            i--;
        }

        public void ClearLeaf() //détruit tout l'arbre
        {
            _leaves.Clear();
            foreach (Leaf leaf in InstantiateLeaves)
            {
                Destroy(leaf.gameObject);
            }

            InstantiateLeaves.Clear();
            i = 0;
        }

        public void printLineRenderer()
        {
            _lineRenderer.positionCount = InstantiateLeaves.Count;
            for (int i = 0; i < InstantiateLeaves.Count; i++)
            {
                _lineRenderer.SetPosition(i, InstantiateLeaves[i]._position);
            }
            
        }
        public void LoadTree()
        {
            List<LeafData> leafDataList = SaveManager.LoadListOfSerializableClass<LeafData>("SaveSkillTree");

            foreach (LeafData leafData in leafDataList)
            {
                GameObject newLeaf = Instantiate(leafPrefab, leaf._position, Quaternion.identity);
                newLeaf.GetComponent<Leaf>()._data = leafData;
                InstantiateLeaves.Add(newLeaf.GetComponent<Leaf>());
            }
        }
        public void Save()
        {
            List<LeafData> DataLeaf = new List<LeafData>();
            foreach (var leaf in InstantiateLeaves)
            {
                DataLeaf.Add(leaf._data);
            }
            
            SaveManager.SaveListOfSerializableClass<LeafData>(DataLeaf, "SaveSkillTree", null, true);
        }

        private void Update()
        {
            printLineRenderer();
        }
    }
}

