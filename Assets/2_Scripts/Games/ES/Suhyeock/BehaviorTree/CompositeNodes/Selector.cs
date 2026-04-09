using System.Collections.Generic;

namespace LUP.ES
{
    public class Selector : BTNode
    {
        private List<BTNode> children = new List<BTNode>();
        private int lastRunningIndex = -1;

        public Selector(List<BTNode> children)
        {
            this.children = children;
        }

        public override NodeState Evaluate()
        {
            for (int i = 0; i < children.Count; i++)
            {
                switch (children[i].Evaluate())
                {
                    case NodeState.Failure:
                        continue;
                    case NodeState.Running:
                        if (lastRunningIndex != -1 && lastRunningIndex != i)
                        {
                            children[lastRunningIndex].Reset();
                        }
                        lastRunningIndex = i;
                        return NodeState.Running;
                    case NodeState.Success:
                        if(lastRunningIndex != -1 && lastRunningIndex != i)
                        {
                            children[lastRunningIndex].Reset();
                        }
                        lastRunningIndex = -1;
                        return NodeState.Success;
                }
            }

            if (lastRunningIndex != -1)
            {
                children[lastRunningIndex].Reset();
                lastRunningIndex = -1;
            }

            return NodeState.Failure;
        }

        public override void Reset()
        {
            lastRunningIndex = -1;
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Reset();
            }
        }
    }
}