using UnityEngine;

namespace RL
{
    public class ActionAttack : LeafNode
    {
        private readonly EnemyBlackBoard bb;
        bool isAnimOnPlayed = false;
        public ActionAttack(EnemyBlackBoard blackBoard, BaseBehaviorTree behaviorTree) : base(blackBoard, behaviorTree )
        {
            bb = blackBoard;
        }

        public override NodeState Evaluate()
        {
            UnityEngine.Debug.Log("Action Attack");
            if (isAnimOnPlayed)
            {
                nodeState = NodeState.Running;
                return nodeState;
            }

            //isAnimOnPlayed = true;
            nodeState = NodeState.Running;

            behaviorTree.PlayAnimation("Attack", this);
            bb.EShooter.ShootArrow(bb.Target, bb.targetPos);
            blackBoard.OnAtk = true;
            blackBoard.InAtkState = true;
            blackBoard.AtkCollTime = blackBoard.AtkCooldownDuration;

            return nodeState;
        }

        public override void OnAnimationEnd(AnimatorStateInfo animInfo)
        {
            UnityEngine.Debug.Log("Hit Animation Ended");
            //isAnimOnPlayed = false;
            nodeState = NodeState.Success;
            blackBoard.InAtkState = false;
            blackBoard.OnAtk = false;
        }
    }
}

