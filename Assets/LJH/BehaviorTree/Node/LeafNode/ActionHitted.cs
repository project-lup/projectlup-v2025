using UnityEngine;

namespace RL
{
    public class ActionHitted : LeafNode
    {
        public override NodeState Evaluate()
        {
            return NodeState.Success;
        }
    }
}

