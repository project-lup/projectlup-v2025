using System.Collections;
using UnityEngine;

namespace LUP.ES
{
    public class ReloadAction : BTNode
    {
        PlayerBlackboard blackboard;
        private bool isReloadingStarted = false;

        public ReloadAction(PlayerBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            if (blackboard.weapon.state == WeaponState.RELOADING)
            {
                return NodeState.Running;
            }
            else if (blackboard.weapon.state == WeaponState.READY && isReloadingStarted)
            {
                isReloadingStarted = false;
                if (blackboard.animator != null)
                {
                    blackboard.animator.SetBool("IsReloading", false);
                }
                return NodeState.Success;
            }

            if (blackboard.weapon is IReloadable reloadableWeapon)
            {
                reloadableWeapon.Reload();
                isReloadingStarted = true;
                blackboard.isReloadButtonPressed = false;
                if (blackboard.animator != null)
                {
                    blackboard.animator.SetBool("IsReloading", true);
                }
                return NodeState.Running;
            }
            return NodeState.Failure;
        }

        public override void Reset()
        {

        }
    }
}
