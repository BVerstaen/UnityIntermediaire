using System;
using System.Collections.Generic;
using GabinBaptisteEnguerrandProject.Scripts;
using UnityEngine;
using LeafData = GabinBaptisteEnguerrandProject.Scripts.LeafData;

namespace GabinBaptisteEnguerrandProject.Prefabs.SkillTree.V2
{
    public class MasteryTree : MonoBehaviour
    {
        [SerializeField] private GameObject BranchPrefab;
        [SerializeField] private Branch _branch;
        private List<Branch> _branches; //toutes les branches de l'arbres

        public List<Branch> _instantiatedBranches;

        // Start is called before the first frame update
        void Awake()
        {
            _branch = BranchPrefab.GetComponent<Branch>();
            _branches = new List<Branch>();
            _instantiatedBranches = new List<Branch>();
        }

        public void AddBranch()
        {
            print(_branch);
            _branch.transform.position = Vector3.zero;
            _branches.Add(_branch);
            _instantiatedBranches.Add(Instantiate(_branch, _branch.transform.position, Quaternion.identity));
        }

        public void LoadTree()
        {
            List<LeafData> leafDataList = SaveManager.LoadListOfSerializableClass<LeafData>("SaveSkillTree");
            int nbBranches = 0;
            foreach (LeafData leafData in leafDataList)
            {
                if (leafData._number > nbBranches)
                    nbBranches = leafData._number;
            }
            print(nbBranches);
            for (int i = 0; i < nbBranches; i++)
            {
                GameObject newBranch = Instantiate(BranchPrefab, _branch.position, Quaternion.identity);
                _instantiatedBranches.Add(newBranch.GetComponent<Branch>());
                _branches.Add(newBranch.GetComponent<Branch>());
            }

            foreach (LeafData leafData in leafDataList)
            {
                foreach (var branch in _instantiatedBranches)
                {
                    GameObject newLeaf = Instantiate(branch.LeafPrefab, branch._leaf._position, Quaternion.identity);
                    newLeaf.GetComponent<Leaves>()._data = leafData;
                    branch._instantiatedleaves.Add(newLeaf.GetComponent<Leaves>());
                    branch._leaves.Add(newLeaf.GetComponent<Leaves>());
                }
            }
        }

        public void Save()
        {
            List<LeafData> DataLeaf = new List<LeafData>();
            foreach (var branch in _instantiatedBranches)
            {
                foreach (var leaf in branch._instantiatedleaves)
                {
                    DataLeaf.Add(leaf._data);
                }
            }

            SaveManager.SaveListOfSerializableClass<LeafData>(DataLeaf, "SaveSkillTree", null, true);
        }
    }
}