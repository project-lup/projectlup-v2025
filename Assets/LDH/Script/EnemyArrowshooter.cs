using UnityEditor.Searcher;
using UnityEngine;

namespace LUP.RL
{
    public class EnemyArrowShooter : MonoBehaviour
    {
        public GameObject ArrowPrefab;
        [Header("EnemyArrowÇÒ´ç")]
        public Transform spawnpoint;

        private float ArrowSpeed = 3;

        private Enemy enemy;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }

        public  void ShootArrow(GameObject  target, Transform TargetTransform)
        {
            spawnpoint.rotation = Quaternion.LookRotation(this.
                transform.forward);
            Vector3 fireDir;
            if(target != null)
            {
                fireDir = (target.transform.position - this.transform.position).normalized;
            }
            else
            {
                fireDir = transform.forward;
            }
            fireDir.y = 0;
            if(fireDir.sqrMagnitude > 0.01f)
            {
                Quaternion LookRot = Quaternion.LookRotation(fireDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, LookRot, 1.5f);

            }
            GameObject Arrow = Instantiate(ArrowPrefab, spawnpoint.position, Quaternion.LookRotation(fireDir));
            EnemyArrow enemyArrow = Arrow.GetComponent<EnemyArrow>();
            enemyArrow.Initialize(target.transform, ArrowSpeed, enemy.EnemyStats.Attack);
          
        }
        // Update is called once per frame
        void Update()
        {

        }
    }

}