using UnityEngine;

namespace LUP.ES
{
    public class MeleeEnemyAnimation : MonoBehaviour
    {

        [SerializeField]
        private EnemyBlackboard blackboard;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void DoAttack()
        {
            blackboard.doAttack = true;

        }

        public void AttackEnd()
        {
            blackboard.AttackEnd = true;

        }
    }
}
