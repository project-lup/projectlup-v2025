using System.Collections.Generic;
using UnityEngine;


public sealed class SelectorNode : INode
{
    List<INode> childList;

    public SelectorNode(List<INode> childs)
    {
        childList = childs;
    }

    // Selector Node는 자식 노드 중에서 처음으로 Success 나 Running 상태를 가진 노드가 발생하면 그 노드까지 진행하고 멈춤
    // 자식 상태: Failure일 때 -> 다음 자식으로 이동
    public INode.WorkerNodeState Evaluate()
    {
        if (childList == null)
            return INode.WorkerNodeState.WNS_FAILURE;

        foreach (var child in childList)
        {
            switch (child.Evaluate())
            {
                case INode.WorkerNodeState.WNS_RUNNING:
                    return INode.WorkerNodeState.WNS_RUNNING;
                case INode.WorkerNodeState.WNS_SUCESS:
                    return INode.WorkerNodeState.WNS_SUCESS;
            
            }

        }
       return INode.WorkerNodeState.WNS_FAILURE;
    }
    
}
