using System.Collections.Generic;

namespace LUP.ES
{
    public class MeleeEnemyBehaviorTree : EnemyBehaviorTreeBase
    {
        protected override Sequence BuildAttackSequence()
        {
            TargetInAttackRangeCondition targetInAttackRangeCondition
                = new TargetInAttackRangeCondition(blackboard);
            TurnToPlayerAction turnToPlayerAction = new TurnToPlayerAction(blackboard);
            EnemyAttackAction enemyAttackAction = new EnemyAttackAction(blackboard);
            Sequence attackSequence = new Sequence(new List<BTNode> 
                { turnToPlayerAction, enemyAttackAction, new WaitAction(2.0f) });

            return new Sequence(new List<BTNode> { targetInAttackRangeCondition, attackSequence });
        }
    }
}
