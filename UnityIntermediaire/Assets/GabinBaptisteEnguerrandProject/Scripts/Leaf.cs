using UnityEngine;

namespace GabinBaptisteEnguerrandProject.Scripts
{
    public class Leaf : MonoBehaviour
    {
        private string _name;
        private int _price;
        private bool _isLocked;
        private Vector3 _position;
        private int _number;

        public Leaf(string name, int price, bool isLocked, Vector3 position, int number)
        {
            this._name = name;
            _price = price;
            _isLocked = isLocked;
            _position = position;
            _number = number;
        }

        
    }
}