using UnityEngine;

namespace ES
{
    public class EnemyDeathAction : BTNode
    {
        EnemyBlackboard blackboard;

        public EnemyDeathAction(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            blackboard.isDead = true;
            return NodeState.Success;
        }

    }
}
