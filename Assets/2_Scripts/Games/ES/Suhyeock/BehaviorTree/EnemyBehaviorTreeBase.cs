using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public abstract class EnemyBehaviorTreeBase : MonoBehaviour
    {
        protected BTNode rootNode;
        protected EnemyBlackboard blackboard;

        protected virtual void Start()
        {
            blackboard = GetComponent<EnemyBlackboard>();
            SetupBehaviorTree();
        }

        protected virtual void Update()
        {
            rootNode.Evaluate();
        }

        private void SetupBehaviorTree()
        {
            // 1. 죽음
            DeadCondition deadCondition = new DeadCondition(blackboard);
            EnemyDeathAction deathAction = new EnemyDeathAction(blackboard);
            Sequence handleDeathSequence = new Sequence(new List<BTNode> { deadCondition, deathAction });

            // 2. 피격
            HitCondition hitCondition = new HitCondition(blackboard);
            EnemyHitAction enemyHitAction = new EnemyHitAction(blackboard);
            Sequence handleHitSequence = new Sequence(new List<BTNode> { hitCondition, enemyHitAction });

            // 3. 최대 이동거리 확인
            TooFarFromHomeCondition tooFarFromHomeCondition = new TooFarFromHomeCondition(blackboard);
            ReturnToInitialPositionAction returnToInitialPositionAction = new ReturnToInitialPositionAction(blackboard);
            Sequence outOfRangeReturnSequence = new Sequence(new List<BTNode> { tooFarFromHomeCondition, returnToInitialPositionAction });


            Sequence handleAttackSequence = BuildAttackSequence();

            // 5. 추격
            TargetInDetectionRangeCondition targetInDetectionRangeCondition = new TargetInDetectionRangeCondition(blackboard);
            ChaseTargetAction chaseTargetAction = new ChaseTargetAction(blackboard);
            Sequence handleMoveSequence = new Sequence(new List<BTNode> { targetInDetectionRangeCondition, chaseTargetAction });

            // 6. 패트롤
            FindRandomLocationAction findRandomLocationAction = new FindRandomLocationAction(blackboard);
            MoveToTargetAction moveToTargetAction = new MoveToTargetAction(blackboard);
            Sequence patrolSequence = new Sequence(new List<BTNode> { findRandomLocationAction, moveToTargetAction, new WaitAction(3.0f) });

            rootNode = new Selector(new List<BTNode>
            {
                handleDeathSequence, handleHitSequence, outOfRangeReturnSequence,
                handleAttackSequence,handleMoveSequence, patrolSequence,
            });
        }
        protected abstract Sequence BuildAttackSequence();

        private void OnDrawGizmos()
        {
            if (Application.isPlaying && blackboard != null && blackboard.transform != null)
            {
                Vector3 attackPoint = blackboard.transform.position + (blackboard.transform.forward * blackboard.attackSize);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(attackPoint, blackboard.attackSize);

                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, blackboard.attackRange);

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, blackboard.detectionRange);

                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(blackboard.initialPosition, blackboard.maxRange);
            }
        }
    }
}
