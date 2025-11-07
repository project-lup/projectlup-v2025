using System.Collections.Generic;

public sealed class SequenceNode : INode
{
    List<INode> childList;

    public SequenceNode(List<INode> childs)
    {
        childList = childs;
    }

    // Sequence Node는 자식 노드를 왼쪽에서 오른쪽으로 진행하면서 Failure 상태가 나올 때까지 진행
    // Running 상태일 때는 그 상태를 계속 유지해야 하기 때문에 다음 자식 노드로 이동하면 안 되고 다음 프레임 때도 그 자식에 대한 평가를 진행
    // 자식 상태: Success 일 때 -> 다음 자식으로 이동
    public INode.WorkerNodeState Evaluate()
  {
      if (childList == null || childList.Count == 0)
            return INode.WorkerNodeState.WNS_FAILURE;

        foreach (var child in childList)
        {
            switch (child.Evaluate())
            {
                case INode.WorkerNodeState.WNS_RUNNING:
                    return INode.WorkerNodeState.WNS_RUNNING;
                case INode.WorkerNodeState.WNS_SUCESS:
                    continue;
                case INode.WorkerNodeState.WNS_FAILURE:
                    return INode.WorkerNodeState.WNS_FAILURE;

            }

        }
        return INode.WorkerNodeState.WNS_SUCESS;
  }
}