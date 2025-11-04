using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    public class EnemyBehaviorTree : MonoBehaviour
    {
        private BTNode rootNode;
        private EnemyBlackboard blackboard;

        private void Awake()
        {
            blackboard = GetComponent<EnemyBlackboard>();
            blackboard.HP = blackboard.MaxHP;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            SetupBehaviorTree();
        }

        // Update is called once per frame
        void Update()
        {
            rootNode.Evaluate();
        }

        private void SetupBehaviorTree()
        {

            DeadCondition deadCondition = new DeadCondition(blackboard);
            EnemyDeathAction deathAction = new EnemyDeathAction(blackboard);
            Sequence handleDeathSequence = new Sequence(new List<BTNode> { deadCondition, deathAction });

            TargetInAttackRangeCondition targetInAttackRangeCondition = new TargetInAttackRangeCondition(blackboard);
            EnemyAttackAction enemyAttackAction = new EnemyAttackAction(blackboard);
            Sequence handleAttackSequence = new Sequence(new List<BTNode> { targetInAttackRangeCondition , enemyAttackAction });

            TargetInDetectionRangeCondition targetInDetectionRangeCondition = new TargetInDetectionRangeCondition(blackboard);
            ChaseTargetAction chaseTargetAction = new ChaseTargetAction(blackboard);
            Sequence handleMoveSequence = new Sequence(new List<BTNode> { targetInDetectionRangeCondition, chaseTargetAction });

            rootNode = new Selector(new List<BTNode> 
            { 
                handleDeathSequence,
                handleAttackSequence,
                handleMoveSequence,
            });
        }
    }
}
