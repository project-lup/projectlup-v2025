using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    [System.Serializable]
    public struct VFXPoolSetting
    {
        public string effectName;
        public GameObject vfxPrefab;
        public int initialCount;
    }

    public class VFXObjectPool : MonoBehaviour
    {
        [Header("Pre-warm Settings")]
        public List<VFXPoolSetting> prewarmSettings = new List<VFXPoolSetting>();

        private Dictionary<int, Queue<GameObject>> poolDict = new Dictionary<int, Queue<GameObject>>();
        private Dictionary<int, GameObject> prefabDict = new Dictionary<int, GameObject>();

        private void Start()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            foreach (var setting in prewarmSettings)
            {
                if (setting.vfxPrefab == null || setting.initialCount <= 0 || string.IsNullOrEmpty(setting.effectName)) continue;

                int hashKey = Animator.StringToHash(setting.effectName);

                if (!poolDict.ContainsKey(hashKey))
                {
                    poolDict[hashKey] = new Queue<GameObject>();
                    prefabDict[hashKey] = setting.vfxPrefab; // 프리팹 원본 캐싱
                }

                for (int i = 0; i < setting.initialCount; i++)
                {
                    GameObject vfx = Instantiate(setting.vfxPrefab, transform);
                    vfx.SetActive(false);
                    poolDict[hashKey].Enqueue(vfx);
                }
            }
        }

        public GameObject SpawnVFX(string effectName, Vector3 position, bool bLoop = false, float duration = 0f)
        {
            if (string.IsNullOrEmpty(effectName)) return null;
            int hashKey = Animator.StringToHash(effectName);
            if (!poolDict.ContainsKey(hashKey)) return null;

            GameObject vfx = poolDict[hashKey].Count > 0 ?
                poolDict[hashKey].Dequeue() : Instantiate(prefabDict[hashKey], transform);
            vfx.transform.position = position;
            vfx.SetActive(true);

            ParticleSystem mainPS = vfx.GetComponent<ParticleSystem>();
            if (mainPS != null)
            {
                var main = mainPS.main;
                main.loop = bLoop;
                mainPS.Play(true);
            }
            if (!bLoop) StartCoroutine(ReturnRoutine(hashKey, vfx, mainPS, duration));
            return vfx;
        }

        public void DespawnVFX(string effectName, GameObject vfx)
        {
            if (string.IsNullOrEmpty(effectName) || vfx == null) return;

            int hashKey = Animator.StringToHash(effectName);

            ParticleSystem mainPS = vfx.GetComponent<ParticleSystem>();
            if (mainPS != null)
            {
                mainPS.Stop(true, ParticleSystemStopBehavior.StopEmitting); // 파티클 부드럽게 정지
            }

            vfx.SetActive(false);

            if (poolDict.ContainsKey(hashKey))
            {
                poolDict[hashKey].Enqueue(vfx);
            }
        }

        private IEnumerator ReturnRoutine(int hashKey, GameObject vfx, ParticleSystem ps, float duration)
        {
            if (duration > 0f) yield return new WaitForSeconds(duration);
            else if (ps != null)
            {
                while (ps.IsAlive(true))
                    yield return null;
            }
            else
                yield return new WaitForSeconds(1.0f);

            vfx.SetActive(false);
            if (poolDict.ContainsKey(hashKey))
                poolDict[hashKey].Enqueue(vfx);
        }
    }
 
}
