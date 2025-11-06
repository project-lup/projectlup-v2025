using UnityEngine;

namespace RL
{
    public class Wait : LeafNode
    {
        public override NodeState Evaluate()
        {
            return NodeState.Success;
        }
    }
}

