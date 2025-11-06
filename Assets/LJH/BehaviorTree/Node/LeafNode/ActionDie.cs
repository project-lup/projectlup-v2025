using UnityEngine;

namespace RL
{
    public class ActionDie : LeafNode
    {
        public override NodeState Evaluate()
        {
            return NodeState.Success;
        }
    }
}

