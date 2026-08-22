using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class RangedEnemyBehaviorTree : EnemyBehaviorTreeBase
    {
        private RangedEnemyBlackboard rangedBlackboard;

        protected override void Start()
        {
            rangedBlackboard = GetComponent<RangedEnemyBlackboard>();
            base.Start();
        }

        protected override void Update()
        {
            if (blackboard.isDetected == true && blackboard.healthComponent.isDead == false)
            {
                TurnToPlayer();
            }
            base.Update();
        }

        protected override Sequence BuildAttackSequence()
        {
            TargetInAttackRangeCondition targetInAttackRangeCondition 
                = new TargetInAttackRangeCondition(blackboard);
            RangedEnemyAttackAction rangedEnemyAttackAction 
                = new RangedEnemyAttackAction(rangedBlackboard, 5);
            Sequence attackSequence = new Sequence(new List<BTNode> 
                { rangedEnemyAttackAction, new WaitAction(2.0f) });

            return new Sequence(new List<BTNode> { targetInAttackRangeCondition, attackSequence });
        }

        void TurnToPlayer()
        {
            Transform enemyTransform = blackboard.navMeshAgent.transform;
            Vector3 targetPosition = blackboard.playerTransform.position;

            Vector3 direction = (targetPosition - enemyTransform.position);
            direction.y = 0;
            direction.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            enemyTransform.rotation = Quaternion.RotateTowards(enemyTransform.rotation, targetRotation, 700.0f * Time.deltaTime);
        }
    }
}
