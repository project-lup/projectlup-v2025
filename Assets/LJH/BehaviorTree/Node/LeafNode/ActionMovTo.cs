using UnityEngine;

namespace RL
{
    public class ActionMovTo : LeafNode
    {
        public override NodeState Evaluate()
        {
            return NodeState.Success;
        }
    }
}

