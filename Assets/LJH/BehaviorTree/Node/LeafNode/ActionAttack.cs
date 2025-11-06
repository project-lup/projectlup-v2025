using UnityEngine;

namespace RL
{
    public class ActionAttack : LeafNode
    {
        public override NodeState Evaluate()
        {
            return NodeState.Success;
        }
    }
}

