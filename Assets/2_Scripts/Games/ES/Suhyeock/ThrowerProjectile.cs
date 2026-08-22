using LUP.ES;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{ 
    public class ThrowerProjectile : MonoBehaviour
    {
        private float damage;
        private float radius;
        public LayerMask targetLayer;
        private BulletObjectPool ownerPool;
        private VFXObjectPool vfxObjectPool;

        [SerializeField] private string explosionEffectName = "ExplosionEffect";

        [SerializeField] private float scaleMultiplier = 1.5f;

        private bool hasExploded = false;
        public void Init(BulletObjectPool objectPool, Vector3 position, Quaternion rotation, float damage, float radius)
        {
            ownerPool = objectPool;
            transform.position = position;
            transform.rotation = rotation;
            this.damage = damage;
            this.radius = radius;
            vfxObjectPool = FindFirstObjectByType<VFXObjectPool>();

            hasExploded = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                return;
            ApplyRadialDamage();
        }

        private void ApplyRadialDamage()
        {
            if (hasExploded) return;
            hasExploded = true;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, targetLayer);

            HashSet<HealthComponent> damagedTargets = new HashSet<HealthComponent>();

            foreach (Collider hit in hitColliders)
            {
                if (hit.TryGetComponent(out HealthComponent healthComponent))
                {
                    if (!damagedTargets.Contains(healthComponent))
                    {
                        healthComponent.TakeDamage(damage);
                        damagedTargets.Add(healthComponent);
                    }
                }
            }

            SoundManager.Instance.PlaySFX("Explosion", gameObject);
            SpawnExplosionVFX();

            ownerPool.Return(gameObject);

        }

        private void SpawnExplosionVFX()
        {
            if (!string.IsNullOrEmpty(explosionEffectName))
            {
                GameObject instance = vfxObjectPool.SpawnVFX(explosionEffectName, transform.position);
                if (instance != null)
                {
                    instance.transform.localScale = Vector3.one * scaleMultiplier;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

}

