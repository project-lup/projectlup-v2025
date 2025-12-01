using UnityEngine;

namespace LUP.ES
{
    public class HitAction : BTNode
    {
        PlayerBlackboard blackboard;

        public HitAction(PlayerBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            blackboard.healthComponent.isHit = false;
            blackboard.eventBroker.CloseLootDisplay();
            blackboard.eventBroker.HandleIventoryVisibility(false);
            blackboard.playerOverheadUI.UpdateHPUI();
            if (blackboard.animator != null)
            {
                blackboard.animator.SetBool("IsInteracting", false);
            }
            return NodeState.Success;
        }

        public override void Reset()
        {

        }
    }
}
