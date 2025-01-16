using System;
using UnityEngine;

namespace GabinBaptisteEnguerrandProject.Scripts
{
    public class Leaf : MonoBehaviour
    {
        public string _name;
        public int _price;
        public bool _isLocked;
        public Vector2 _position;
        public int _number;
        public Leaf _previousLeaf;
        
        private Material _material;
        private void Start()
        {
            _material = GetComponent<Renderer>().material;
        }


        private void Update()
        {
            if (!_isLocked)
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