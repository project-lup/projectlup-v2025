using UnityEngine;

namespace ES
{
    public class EnemyGun : MonoBehaviour
    {
        public ItemDataBase itemDataBase; //임시
        public int selectedWeaponId = 1; //임시
        public Weapon weapon;
        public GameObject bulletPrefab;
        private BulletObjectPool bulletPool;
        public Transform firePoint;
        private float nextFireTime = 0f;

        private void Awake()
        {
            bulletPool = GetComponent<BulletObjectPool>();
        }


        private void Start()
        {
            BaseItemData itemData = itemDataBase.GetItemByID(selectedWeaponId);
            WeaponItemData weaponData = itemData as WeaponItemData;
            weapon = new Weapon(weaponData);
            bulletPool.Init(bulletPrefab);
        }

        public bool Fire()
        {

            if (Time.time < nextFireTime)
            {
                return false;
            }

            nextFireTime = Time.time + weapon.timeBetFire;
            GameObject obj = bulletPool.Get();
            Bullet bullet = obj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Init(bulletPool, firePoint.position, firePoint.rotation, weapon.range, weapon.damage, weapon.bulletSpeed);
                return true;
            }
            return true;
        }
    }
}


