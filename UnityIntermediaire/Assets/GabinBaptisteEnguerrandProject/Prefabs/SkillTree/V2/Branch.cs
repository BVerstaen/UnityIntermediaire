using System.Collections.Generic;
using UnityEngine;

namespace GabinBaptisteEnguerrandProject.Prefabs.SkillTree.V2
{
    public class Branch : MonoBehaviour
    {
        public List<Leaves> _leaves; //toutes les feuilles de la branche
        public List<Leaves> _instantiatedleaves; //toutes les feuilles de la branche
        [SerializeField] public GameObject LeafPrefab;
        [SerializeField] public Leaves _leaf;

        public Vector2 position;
        public int price; //prix pour unlock
        public bool isLocked; //lock la feuille
        private LineRenderer _lineRenderer;

        // Start is called before the first frame update
        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _leaf = LeafPrefab.GetComponent<Leaves>();
            _leaves = new List<Leaves>();
            _instantiatedleaves = new List<Leaves>();
        }

        public void AddLeaf()
        {
            _leaves.Add(_leaf);
            _instantiatedleaves.Add(Instantiate(_leaf, _leaf.transform.position, Quaternion.identity));
        }

        public void SetupLeaf(Vector2 position, int price, bool isLocked, int nbOfBranch)
        {
            _leaf._position = position;
            _leaf._price = price;
            _leaf._isLocked = isLocked;
            _leaf._number = nbOfBranch;
        }

        public void AddLeaf(Leaves leaf)
        {
            leaf._number = _leaf._number + 1;
            _leaves.Add(leaf);
            _instantiatedleaves.Add(leaf);
            //_instantiatedleaves.Add(Instantiate(leaf, leaf.transform.position, Quaternion.identity));
        }

        public static int GetAllLeavesInBranch(List<Leaves> leaves)
        {
            foreach (Leaves leaf in leaves)
            {
                return leaf._number;
            }

            return -1;
        }

        public void LoadLeaves()
        {
        }

        public void PrintLineRenderer()
        {
            _lineRenderer.positionCount = _instantiatedleaves.Count;
            for (int i = 0; i < _instantiatedleaves.Count; i++)
            {
                _lineRenderer.SetPosition(i, _instantiatedleaves[i]._position);
            }
        }

        // Update is called once per frame
        void Update()
        {
            PrintLineRenderer();
        }
    }
}