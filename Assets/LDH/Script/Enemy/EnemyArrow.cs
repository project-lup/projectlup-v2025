
using UnityEngine;
namespace RL
{
    public class  EnemyArrow : MonoBehaviour
    {
        public Transform target;
        public float speed;
        public float LifeTime = 5f;
        private int damage;
      
        private void Start()
        {
            Destroy(gameObject, LifeTime);
        }
        public void Initialize(Transform target, float speed, int damage)
        {
            this.target = target;
            this.speed = speed;
            this.damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("충돌");
                Destroy(gameObject);
                //enemy.TakeDamage(archer.Adata.currentData.Attack);
            }
        }
        void Update()
        {

            //if (target == null)
            //{
            //    transform.position += transform.forward * speed * Time.deltaTime;
            //    return;
            //}

            //// 적을 향한 방향 벡터 계산
            //Vector3 dir = (target.position - transform.position).normalized;

            //// 회전은 안 하고, 방향 벡터로 직접 이동
            //transform.position += dir * speed * Time.deltaTime;
        }


    }
}
