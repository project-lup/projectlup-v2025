using UnityEngine;

namespace LUP.RL
{
    public abstract class LeafNode : Node
    {
        protected BaseBehaviorTree behaviorTree;
        protected BlackBoard blackBoard;

        public LeafNode(BlackBoard blackBoard, BaseBehaviorTree behaviorTree)
        {
            this.blackBoard = blackBoard;
            this.behaviorTree = behaviorTree;
        }

        public abstract void OnAnimationEnd(AnimatorStateInfo animInfo);
    }
}

