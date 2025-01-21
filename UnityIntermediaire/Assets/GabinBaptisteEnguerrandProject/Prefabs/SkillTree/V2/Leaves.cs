using System;
using System.Collections.Generic;
using GabinBaptisteEnguerrandProject.Scripts;
using UnityEngine;

namespace GabinBaptisteEnguerrandProject.Prefabs.SkillTree.V2
{
    public class Leaves : MonoBehaviour
    {
        public LeafData _data;
        [SerializeField] private GameObject BranchPrefab;
        private Branch _branch;
        private List<Branch> _branchesInLeaves;
        private List<Branch> _instantiatedBranches;
        private Material _material;

        public string _name
        {
            get => _data._name;
            set => _data._name = value;
        }

        public int _price
        {
            get => _data._price;
            set => _data._price = value;
        }

        public bool _isLocked
        {
            get => _data._isLocked;
            set => _data._isLocked = value;
        }

        public Vector2 _position
        {
            get => _data._position;
            set => _data._position = value;
        }

        public int _number
        {
            get => _data._number;
            set => _data._number = value;
        }

        private void Start()
        {
            this.gameObject.transform.position = this._position;
            _branch = BranchPrefab.GetComponent<Branch>();
            _branchesInLeaves = new List<Branch>();
            _instantiatedBranches = new List<Branch>();
            _material = GetComponent<MeshRenderer>().material;
        }


        public void AddBranch()
        {
            _branchesInLeaves.Add(_branch);
            _branch.transform.position = _position;
            _instantiatedBranches.Add(Instantiate(_branch, _branch.transform.position, Quaternion.identity));
            _instantiatedBranches[0].SetupLeaf(this._position, this._price, this._isLocked);
            _instantiatedBranches[0].AddLeaf(this);
        }


        private void Update()
        {
            transform.position = _position;
            if (!_data._isLocked)
            {
                _material.color = Color.green;
            }
            else
            {
                _material.color = Color.yellow;
            }


            _position = transform.position;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ObjectInteraction>() != null)
            {
                var interaction = other.GetComponent<ObjectInteraction>();
                if (interaction._money >= _data._price)
                {
                    interaction._money -= _data._price;
                    UnlockLeaf();
                }
            }
        }

        public void UnlockLeaf()
        {
            _data._isLocked = false;
        }
    }
}