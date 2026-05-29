using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class BulletObjectPool : MonoBehaviour
    {
        private GameObject bullletPrefab;
        public int initialSize = 20;
        private int currentCreatedCount = 0;

        Queue<GameObject> pool = new Queue<GameObject>();
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Init(GameObject bullletPrefab)
        {
            this.bullletPrefab = bullletPrefab;
            for (int i = 0; i < initialSize; i++)
            {
                CreateNewBullet();
            }
        }

        private void CreateNewBullet()
        {
            GameObject bullet = Instantiate(bullletPrefab);
            bullet.SetActive(false);
            pool.Enqueue(bullet);
            currentCreatedCount++;
        }

        public GameObject Get()
        {
            if (pool.Count == 0)
            {
                CreateNewBullet();
            }

            GameObject bullet = pool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }

        public void Return(GameObject bullet)
        {
            bullet.SetActive(false);
            pool.Enqueue(bullet);
        }

        public void PoolDestroy()
        {
            while (pool.Count > 0)
            {
                GameObject bullet = pool.Dequeue();
                if (bullet != null)
                {
                    Destroy(bullet);
                }
            }
        }
    }
}
