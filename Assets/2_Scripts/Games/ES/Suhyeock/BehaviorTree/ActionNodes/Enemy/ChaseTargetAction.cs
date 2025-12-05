using UnityEngine;
using UnityEngine.AI;

namespace LUP.ES
{
    public class ChaseTargetAction : BTNode
    {
        EnemyBlackboard blackboard;

        public ChaseTargetAction(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            blackboard.navMeshAgent.SetDestination(blackboard.playerTransform.position);
            blackboard.ChangeState(EnemyState.Run);
            return NodeState.Success;
        }

        public override void Reset()
        {
            
        }
    }
}
