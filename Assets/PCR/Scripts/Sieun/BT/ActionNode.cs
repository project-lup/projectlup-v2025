using System;
using UnityEngine;

public sealed class ActionNode : INode
{
    Func<INode.WorkerNodeState> _onUpdate = null;
   public ActionNode(Func<INode.WorkerNodeState> onUpdate)
    {
        // 생성자의 매개변수를 통해 액션을 연결하는 Func 델리게이트
        _onUpdate = onUpdate; // onUpdate = ActionNode(액션 노드)
    }

    // return NodeState
    // ?? (널 병합 연산자)는 왼쪽의 값이 null일 경우 오른쪽의 값을 대신 사용
    INode.WorkerNodeState INode.Evaluate() => _onUpdate?.Invoke() ?? INode.WorkerNodeState.WNS_FAILURE;

}
