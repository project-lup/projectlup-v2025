using ES;
using JetBrains.Annotations;
using System;
using UnityEngine;
namespace RL
{
    public class Enemy : MonoBehaviour
    {
        public BaseStats EnemyStats;
        public int expValue = 10;
        public EnemyCenter enemyCTR;

        public Vector2Int gridPos;
        public static event Action<int> OnEnemyDied;
        private EnemyBlackBoard enemyBlackBoard;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (enemyCTR == null)
            {
                Debug.LogError($"{name} : enemyCTR가 null입니다. EnemyCenter에 연결되지 않았습니다!");
                return;
            }
            enemyCTR.enemies.Add(this);
            EnemyStats.Hp = 500;
            EnemyStats.Attack = 0;
            EnemyStats.speed = 3;

            enemyBlackBoard = GetComponentInChildren<EnemyBlackBoard>();
        }
        public  void SetGridPos(int x, int z)
        {
            gridPos = new Vector2Int(x, z);
        }
        public void TakeDamage(int damage)
        {
            EnemyStats.Hp -= damage;
            Debug.Log($"데미지 : {damage} 남은체력 {EnemyStats.Hp}");

            //enemyBlackBoard.OnHitted = true;

            if (EnemyStats.Hp <= 0)
            {
                Die();
            }
        }
        private void Die()
        {
            OnEnemyDied?.Invoke(expValue);
            

            enemyCTR?.ObjectOnEnemyDied?.Invoke(this);
            Destroy(gameObject);
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}