using UnityEngine;

namespace LUP.ES
{
    public class RangedEnemyAttackAction : BTNode
    {
        RangedEnemyBlackboard blackboard;
        private const float TURN_SPEED = 500.0F;
        int shotsPerBurst = 5;
        int totalShotsFired = 0;

        public RangedEnemyAttackAction(RangedEnemyBlackboard blackboard, int shotsPerBurst)
        {
            this.blackboard = blackboard;
            this.shotsPerBurst = shotsPerBurst;
        }

        public override NodeState Evaluate()
        {

            if (blackboard.gun.Fire())
            {
                blackboard.ChangeState(EnemyState.Attack);
                totalShotsFired++;
            }
            
            if(totalShotsFired >= shotsPerBurst)
            {
                blackboard.ChangeState(EnemyState.Idle);
                totalShotsFired = 0;
                return NodeState.Success;
            }

            return NodeState.Running;
        }

        public override void Reset()
        {
            totalShotsFired = 0;
        }
    }
}

