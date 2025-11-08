using UnityEngine;

namespace ES
{
    public class EnemyDeathAction : BTNode
    {
        EnemyBlackboard blackboard;
        float deathTime = 2f;

        public EnemyDeathAction(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            blackboard.navMeshAgent.speed = 0;
            deathTime -= Time.deltaTime;
            if (deathTime < 0)
            {
                
                Object.Destroy(blackboard.gameObject);
            }
            return NodeState.Running;
        }

        public override void Reset()
        {

        }
    }
}
