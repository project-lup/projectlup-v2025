using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class BulletObjectPool : MonoBehaviour
    {
        private GameObject bulletPrefab;
        [SerializeField]private int initialSize = 20;
        Queue<GameObject> pool = new Queue<GameObject>();

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

        public void Init(GameObject bulletPrefab)
        {
            this.bulletPrefab = bulletPrefab;
            for (int i = 0; i < initialSize; i++)
            {
                CreateNewBullet();
            }
        }

        private void CreateNewBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab);
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
