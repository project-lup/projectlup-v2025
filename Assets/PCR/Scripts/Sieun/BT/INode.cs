using UnityEngine;

public interface INode
{
    public enum WorkerNodeState
    {
        WNS_RUNNING,
        WNS_SUCESS,
        WNS_FAILURE,
    }

    public WorkerNodeState Evaluate();
}
