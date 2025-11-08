using JetBrains.Annotations;
using System;
using UnityEngine;
namespace RL
{
    public class Enemy : MonoBehaviour
    {
        public BaseStats EnemyStats;
        public int expValue = 10;
        public static event Action<int> OnEnemyDied;
        public delegate void EnemyDeathHandler(Enemy deadEnemy);
        public static event EnemyDeathHandler ObjectOnEnemyDied;
        void Start()
        {
            EnemyStats.Hp = 1000;
            EnemyStats.Attack = 10;
            EnemyStats.speed = 3;
        }
  
        public void TakeDamage(int damage)
        {
            EnemyStats.Hp -= damage;
            Debug.Log($"데미지 : {damage} 남은체력 {EnemyStats.Hp}");
            if (EnemyStats.Hp <= 0)
            {
                Die();
            }
        }
        private void Die()
        {
            OnEnemyDied?.Invoke(expValue);

            ObjectOnEnemyDied?.Invoke(this);


            Destroy(gameObject);
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}