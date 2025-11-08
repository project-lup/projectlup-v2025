using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RL
{
    public class EnemyCenter : MonoBehaviour
    {
        public List<Enemy> enemies = new();
        public delegate void EnemyDeathHandler(Enemy deadEnemy);
        public event EnemyDeathHandler ObjectOnEnemyDied;
      
        private void Start()
        {
            //// 스테이지 시작 시 등록된 적 리스트 초기화
            //enemies.AddRange(FindFirstObjectByType<Enemy>());
            //foreach (var enemy in enemies)
            //    enemy.OnEnemyDie += HandleEnemyDeath;
        }

        private void HandleEnemyDeath(Enemy deadEnemy)
        {
            enemies.Remove(deadEnemy);
            if (enemies.Count == 0)
            {
                //EventBus.PublishStageClear();
            }
        }
    }
}
