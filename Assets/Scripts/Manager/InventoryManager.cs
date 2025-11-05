using System;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("아이템 데이터베이스")]
        [SerializeField] private string itemDatabasePath = "Items"; // Resources 폴더 경로

        // 현재 활성 인벤토리
        private MonoBehaviour currentInventory;

        // 아이템 데이터베이스 (ID → IInventoryItem)
        private Dictionary<int, IInventoryItem> itemDatabase;

        // 전역 이벤트
        public event Action<IInventoryItem, int> OnAnyItemAdded;
        public event Action<IInventoryItem, int> OnAnyItemRemoved;
        public event Action OnAnyInventoryUpdated;

        private bool isDatabaseLoaded = false;

        public override void Awake()
        {
            base.Awake();
            InitializeItemDatabase();
        }

        #region 인벤토리 등록/관리

        /// <summary>
        /// 인벤토리 등록 (각 게임 씬에서 호출)
        /// </summary>
        public void RegisterInventory(MonoBehaviour inventory)
        {
            currentInventory = inventory;
            Debug.Log($"[InventoryManager] Registered: {inventory.GetType().Name}");
        }

        /// <summary>
        /// 현재 활성 인벤토리 가져오기
        /// </summary>
        public T GetCurrentInventory<T>() where T : MonoBehaviour
        {
            if (currentInventory == null)
            {
                Debug.LogWarning("[InventoryManager] No inventory registered");
                return null;
            }

            if (currentInventory is T inventory)
            {
                return inventory;
            }

            Debug.LogError($"[InventoryManager] Current inventory is not of type {typeof(T).Name}");
            return null;
        }

        /// <summary>
        /// 현재 인벤토리가 등록되어 있는지 확인
        /// </summary>
        public bool HasInventory()
        {
            return currentInventory != null;
        }

        #endregion

        #region 아이템 데이터베이스

        /// <summary>
        /// 아이템 데이터베이스 초기화
        /// Resources/Items 폴더의 모든 BaseItemData 로드
        /// </summary>
        private void InitializeItemDatabase()
        {
            if (isDatabaseLoaded)
            {
                return;
            }

            itemDatabase = new Dictionary<int, IInventoryItem>();

            // Resources 폴더에서 모든 BaseItemData 로드
            Framework.BaseItemData[] items = Resources.LoadAll<Framework.BaseItemData>(itemDatabasePath);

            foreach (var item in items)
            {
                if (itemDatabase.ContainsKey(item.ItemID))
                {
                    Debug.LogWarning($"[InventoryManager] Duplicate ItemID: {item.ItemID} ({item.ItemName})");
                    continue;
                }

                itemDatabase[item.ItemID] = item;
            }

            isDatabaseLoaded = true;
            Debug.Log($"[InventoryManager] Loaded {itemDatabase.Count} items from database");
        }

        /// <summary>
        /// 아이템 ID로 아이템 데이터 가져오기
        /// </summary>
        public IInventoryItem GetItemData(int itemID)
        {
            if (!isDatabaseLoaded)
            {
                InitializeItemDatabase();
            }

            if (itemDatabase.TryGetValue(itemID, out var item))
            {
                return item;
            }

            Debug.LogError($"[InventoryManager] Item not found: ID {itemID}");
            return null;
        }

        /// <summary>
        /// 아이템 ID로 타입 변환하여 가져오기
        /// </summary>
        public T GetItemData<T>(int itemID) where T : class, IInventoryItem
        {
            IInventoryItem item = GetItemData(itemID);
            return item as T;
        }

        /// <summary>
        /// 아이템이 데이터베이스에 존재하는지 확인
        /// </summary>
        public bool ItemExists(int itemID)
        {
            if (!isDatabaseLoaded)
            {
                InitializeItemDatabase();
            }

            return itemDatabase.ContainsKey(itemID);
        }

        /// <summary>
        /// 특정 타입의 모든 아이템 가져오기
        /// </summary>
        public List<IInventoryItem> GetItemsByType(Define.ItemType itemType)
        {
            if (!isDatabaseLoaded)
            {
                InitializeItemDatabase();
            }

            List<IInventoryItem> result = new List<IInventoryItem>();

            foreach (var item in itemDatabase.Values)
            {
                if (item.ItemType == itemType)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// 전체 아이템 목록 가져오기
        /// </summary>
        public List<IInventoryItem> GetAllItems()
        {
            if (!isDatabaseLoaded)
            {
                InitializeItemDatabase();
            }

            return new List<IInventoryItem>(itemDatabase.Values);
        }

        #endregion

        #region 저장/로드

        /// <summary>
        /// 현재 인벤토리 저장
        /// </summary>
        public void SaveInventory(string saveKey = "CurrentInventory")
        {
            if (currentInventory == null)
            {
                Debug.LogWarning("[InventoryManager] No inventory to save");
                return;
            }

            // BaseInventory를 상속하는지 확인하고 저장 메서드 호출
            var saveMethod = currentInventory.GetType().GetMethod("ToSaveData");
            if (saveMethod == null)
            {
                Debug.LogError($"[InventoryManager] {currentInventory.GetType().Name} does not implement ToSaveData()");
                return;
            }

            InventoryRuntimeData data = saveMethod.Invoke(currentInventory, null) as InventoryRuntimeData;
            if (data != null)
            {
                JsonDataHelper.SaveData(data, $"{saveKey}.json");
                Debug.Log($"[InventoryManager] Inventory saved: {saveKey}");
            }
        }

        /// <summary>
        /// 현재 인벤토리 로드
        /// </summary>
        public void LoadInventory(string saveKey = "CurrentInventory")
        {
            if (currentInventory == null)
            {
                Debug.LogWarning("[InventoryManager] No inventory to load into");
                return;
            }

            string filename = $"{saveKey}.json";

            if (!JsonDataHelper.FileExists(filename))
            {
                Debug.LogWarning($"[InventoryManager] Save file not found: {filename}");
                return;
            }

            InventoryRuntimeData data = JsonDataHelper.LoadData<InventoryRuntimeData>(filename);

            if (data != null)
            {
                var loadMethod = currentInventory.GetType().GetMethod("FromSaveData");
                if (loadMethod != null)
                {
                    loadMethod.Invoke(currentInventory, new object[] { data });
                    Debug.Log($"[InventoryManager] Inventory loaded: {saveKey}");
                }
            }
        }

        /// <summary>
        /// 스테이지 전환 시 자동 저장 (StageManager와 연동)
        /// </summary>
        public void OnStageExit()
        {
            if (currentInventory == null)
            {
                return;
            }

            // ShouldPersist 체크
            var persistProperty = currentInventory.GetType().GetProperty("ShouldPersist");
            if (persistProperty != null)
            {
                bool shouldPersist = (bool)persistProperty.GetValue(currentInventory);
                if (shouldPersist)
                {
                    SaveInventory($"Inventory_{StageManager.Instance.GetCurrentStage()?.GetType().Name}");
                }
            }
        }

        /// <summary>
        /// 특정 스테이지의 인벤토리 데이터 삭제
        /// </summary>
        public void DeleteInventorySave(string saveKey)
        {
            string filename = $"{saveKey}.json";
            if (JsonDataHelper.FileExists(filename))
            {
                JsonDataHelper.DeleteData(filename);
                Debug.Log($"[InventoryManager] Deleted save: {saveKey}");
            }
        }

        #endregion

        #region 전역 이벤트 브로드캐스트

        /// <summary>
        /// 아이템 추가 이벤트 브로드캐스트
        /// </summary>
        public void BroadcastItemAdded(IInventoryItem item, int amount)
        {
            OnAnyItemAdded?.Invoke(item, amount);
            OnAnyInventoryUpdated?.Invoke();
        }

        /// <summary>
        /// 아이템 제거 이벤트 브로드캐스트
        /// </summary>
        public void BroadcastItemRemoved(IInventoryItem item, int amount)
        {
            OnAnyItemRemoved?.Invoke(item, amount);
            OnAnyInventoryUpdated?.Invoke();
        }

        #endregion

        #region 유틸리티

        /// <summary>
        /// 데이터베이스 리로드 (에디터 전용)
        /// </summary>
        public void ReloadDatabase()
        {
            isDatabaseLoaded = false;
            InitializeItemDatabase();
            Debug.Log("[InventoryManager] Database reloaded");
        }

        /// <summary>
        /// 디버그 정보 출력
        /// </summary>
        public void DebugPrintStatus()
        {
            Debug.Log("===== InventoryManager Status =====");
            Debug.Log($"Current Inventory: {(currentInventory != null ? currentInventory.GetType().Name : "None")}");
            Debug.Log($"Database Loaded: {isDatabaseLoaded}");
            Debug.Log($"Total Items in Database: {itemDatabase?.Count ?? 0}");
        }

        #endregion
    }
}
