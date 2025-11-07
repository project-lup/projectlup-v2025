using UnityEngine;

public class BehaviorTreeRunner
{
    INode _rootNode;
    public BehaviorTreeRunner(INode rootNode)
    {
        _rootNode = rootNode;
    }

    public void Operate()
    {
        _rootNode.Evaluate(); // WorkerAI의 Update 루프 안에서 호출된다.
    }
}