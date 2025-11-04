using UnityEngine;

namespace ES
{
    public class EnemyAttackAction : BTNode
    {
        EnemyBlackboard blackboard;
        public EnemyAttackAction(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            blackboard.navMeshAgent.ResetPath();
            Debug.Log("Attack");
            return NodeState.Success;
        }
    }
}

    
