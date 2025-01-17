using System;
using System.Collections.Generic;
using UnityEngine;

namespace GabinBaptisteEnguerrandProject.Scripts
{
    [Serializable]
    public class LeafData
    {
        public string _name;
        public int _price;
        public bool _isLocked;
        public Vector2 _position;
        public int _number;
        public Leaf _previousLeaf;

    }

    public class Leaf : MonoBehaviour
    {
        public LeafData _data;

        public string _name { get => _data._name; set => _data._name = value; }
        public int _price { get => _data._price; set => _data._price = value; }
        public bool _isLocked { get => _data._isLocked; set => _data._isLocked = value; }
        public Vector2 _position { get => _data._position; set => _data._position = value; }
        public int _number { get => _data._number; set => _data._number = value; }
        public Leaf _previousLeaf { get => _data._previousLeaf; set => _data._previousLeaf = value; }

        public Leaf(Leaf leaf)
        {
            _data = leaf._data;
        }

        private Material _material;
        private void Start()
        {
            _material = GetComponent<Renderer>().material;
            
            transform.position = _position;
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
                _material.color = Color.red;
            }
        }
    }
}