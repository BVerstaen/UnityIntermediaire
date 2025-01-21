using System;
using System.Collections.Generic;
using UnityEngine;

namespace GabinBaptisteEnguerrandProject.Prefabs.SkillTree.V2
{
    public class MasteryTree : MonoBehaviour
    {
        [SerializeField] private GameObject BranchPrefab;
        [SerializeField] private Branch _branch;
        private List<Branch> _branches; //toutes les branches de l'arbres

        private List<Branch> _instantiatedBranches;

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

        public void LoadBranch()
        {
        }
    }
}