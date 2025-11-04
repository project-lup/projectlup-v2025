using UnityEngine;
using UnityEngine.AI;

namespace ES
{
    public class EnemyBlackboard : BaseBlackboard
    {
        public float damage = 10f;
        public float attackRange = 2f;
        public float detectionRange = 10f;
        public Transform playerTransform;

        public NavMeshAgent navMeshAgent;


        public void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = speed;
        }
    }
}
