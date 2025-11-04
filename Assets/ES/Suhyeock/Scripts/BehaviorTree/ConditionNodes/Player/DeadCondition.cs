using UnityEngine;

namespace ES
{
    public class DeadCondition : BTNode
    {
        BaseBlackboard blackboard;

        public DeadCondition(BaseBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            if(blackboard.HP <= 0)
            {
                return NodeState.Success;
            } 
            return NodeState.Failure;
        }
    }
}
