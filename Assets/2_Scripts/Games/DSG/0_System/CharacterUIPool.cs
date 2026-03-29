using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LUP.DSG
{
    public class CharacterUIPool : MonoBehaviour
    {
        [SerializeField] 
        private CharacterHeadupUI uiPrefab;
        [SerializeField]
        private Canvas targetCanvas;

        private Transform uiRoot;
        private IObjectPool<CharacterHeadupUI> pool;
        public Canvas TargetCanvas => targetCanvas;

        void Awake()
        {
            ResolveCanvas();

            pool = new ObjectPool<CharacterHeadupUI>(
                createFunc: CreatePooledItem,
                actionOnGet: OnGetFromPool,
                actionOnRelease: OnReturnedToPool,
                actionOnDestroy: OnDestroyPoolObject,
                collectionCheck: true,
                defaultCapacity: 10,
                maxSize: 20
            );
        }

        private CharacterHeadupUI CreatePooledItem()
        {
            CharacterHeadupUI uiInstance = Instantiate(uiPrefab, uiRoot);
            return uiInstance;
        }

        private void OnGetFromPool(CharacterHeadupUI ui)
        {
            ui.gameObject.SetActive(true);
        }

        private void OnReturnedToPool(CharacterHeadupUI ui)
        {
            ui.ReleaseTarget();
        }

        private void OnDestroyPoolObject(CharacterHeadupUI ui)
        {
            Destroy(ui.gameObject);
        }

        public CharacterHeadupUI GetUI(Transform target, Vector3 uiOffset)
        {
            ResolveCanvas();
            if (targetCanvas == null || uiPrefab == null) return null;

            CharacterHeadupUI uiObject = pool.Get();
            if (uiObject == null) return null;

            uiObject.transform.SetParent(uiRoot, false);
            uiObject.SetTarget(targetCanvas, target, uiOffset);

            return uiObject;
        }

        public void Release(CharacterHeadupUI ui)
        {
            if (ui == null) return;

            pool.Release(ui);

            ResolveCanvas();
            if (uiRoot != null) ui.transform.SetParent(uiRoot, false);
        }

        private void ResolveCanvas()
        {
            if (targetCanvas == null)
                targetCanvas = GameObject.Find("Canvas_CharacterUI")?.GetComponent<Canvas>();

            if (uiRoot == null && targetCanvas != null)
                uiRoot = targetCanvas.transform;
        }
    }
}