using UnityEngine;

namespace RL
{
    public class ReduceHP : LeafNode
    {
        public override NodeState Evaluate()
        {
            return NodeState.Success;
        }
    }
}

