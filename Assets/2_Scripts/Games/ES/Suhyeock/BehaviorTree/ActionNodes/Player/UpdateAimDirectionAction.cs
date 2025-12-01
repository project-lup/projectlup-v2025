using UnityEngine;

namespace LUP.ES
{
    public class UpdateAimDirectionAction : BTNode 
    {
        PlayerBlackboard blackboard;
        public UpdateAimDirectionAction(PlayerBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            float horizontal = blackboard.rightJoystick.Horizontal;
            float Vertical = blackboard.rightJoystick.Vertical;
            if (horizontal != 0 || Vertical != 0)
            {
                Vector3 dir = new Vector3(horizontal, 0f, Vertical);
                dir.Normalize();
                blackboard.transform.forward = dir;
            }
            return NodeState.Success;
        }

        public override void Reset()
        {
            
        }

    }

}
