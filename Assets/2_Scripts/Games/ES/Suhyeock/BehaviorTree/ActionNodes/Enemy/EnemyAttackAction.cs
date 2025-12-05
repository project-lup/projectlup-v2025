using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class EnemyAttackAction : BTNode
    {
        EnemyBlackboard blackboard;
        public EnemyAttackAction(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            blackboard.navMeshAgent.ResetPath();

            if (blackboard.doAttack)
            {
                Vector3 enemyPosition = blackboard.transform.position;
                Vector3 enemyForward = blackboard.transform.forward;

                Quaternion enemyQuaternion = blackboard.transform.rotation;

                Vector3 attackPoint = enemyPosition + (enemyForward * blackboard.attackSize);

                Collider[] hitColliders = Physics.OverlapSphere(
                    attackPoint,
                    blackboard.attackSize,
                    blackboard.LayerMask
                );

                HashSet<GameObject> hitTargetsOnce = new HashSet<GameObject>();

                for (int i = 0; i < hitColliders.Length; i++)
                {
                    GameObject rootObject = hitColliders[i].gameObject;
                    if (hitTargetsOnce.Add(rootObject))
                    {
                        HealthComponent health = hitColliders[i].GetComponent<HealthComponent>();

                        if (health)
                        {
                            health.TakeDamage(blackboard.damage);
                            Debug.Log("Attack");
                        }
                    }
                }
                blackboard.doAttack = false;
                return NodeState.Running;
            }
            else
            {
                if (blackboard.AttackEnd)
                {
                    blackboard.AttackEnd = false;
                    blackboard.ChangeState(EnemyState.Idle);
                    return NodeState.Success;
                }
            }

            blackboard.ChangeState(EnemyState.Attack);
            return NodeState.Running;
        }

        public override void Reset()
        {

        }
    }
}

    
