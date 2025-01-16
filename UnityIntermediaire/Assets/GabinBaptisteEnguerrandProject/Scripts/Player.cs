using UnityEngine;

namespace GabinBaptisteEnguerrandProject.Scripts
{
    public class Player : MonoBehaviour
    {
        public int _money;
        public SkillTree _skillTree;


        public void BuySkill()
        {
            _skillTree.UnlockLeaf(0,_money);
        }
    }
}