using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP
{
    public class ItemManager : Singleton<ItemManager>
    {
        [Header("데이터 로더 목록 (여러 게임 시트)")]
        [SerializeField] private List<BaseStaticDataLoader> loaders = new List<BaseStaticDataLoader>();

        private Dictionary<int, LUPItemData> itemDatabase = new Dictionary<int, LUPItemData>();
        private bool isLoaded = false;

        public override void Awake()
        {
            base.Awake();

            if (loaders.Count == 0)
            {
                FindAllLoaders();
            }
        }

        private void FindAllLoaders()
        {
            var foundLoaders = Resources.LoadAll<BaseStaticDataLoader>("Data/StaticData");
            loaders.AddRange(foundLoaders);

            if (loaders.Count > 0)
            {
                Debug.Log($"[ItemManager] {loaders.Count}개 로더 발견");
            }
        }

        public IEnumerator LoadAllItems()
        {
            itemDatabase.Clear();

            if (loaders.Count == 0)
            {
                Debug.LogWarning("[ItemManager] 로더가 없습니다. 자동으로 찾습니다...");
                FindAllLoaders();
            }

            foreach (var loader in loaders)
            {
                if (loader == null)
                {
                    Debug.LogWarning("[ItemManager] null 로더 발견, 건너뜁니다.");
                    continue;
                }

                // BaseStaticDataLoader를 통해 DataList 접근
                var dataListProp = loader.GetType().GetProperty("DataList");
                if (dataListProp == null)
                {
                    Debug.LogWarning($"[ItemManager] {loader.name}에 DataList 프로퍼티가 없습니다.");
                    continue;
                }

                var dataList = dataListProp.GetValue(loader) as System.Collections.IList;
                if (dataList == null || dataList.Count == 0)
                {
                    Debug.LogWarning($"[ItemManager] {loader.name}의 DataList가 비어있습니다. '데이터 읽기'를 먼저 실행하세요.");
                    continue;
                }

                Debug.Log($"[ItemManager] {loader.name}에서 {dataList.Count}개 아이템 로드 중...");

                foreach (var staticData in dataList)
                {
                    // LUPItemStaticData인지 확인
                    if (staticData is LUPItemStaticData itemStaticData)
                    {
                        var itemData = itemStaticData.ToItemData();

                        // 같은 ID 아이템이 이미 있으면 병합 (공유 아이템 케이스)
                        if (itemDatabase.ContainsKey(itemData.ItemID))
                        {
                            Debug.Log($"[ItemManager] 아이템 병합: {itemData.ItemName} (ID: {itemData.ItemID})");
                            itemDatabase[itemData.ItemID].MergeWith(itemData);
                        }
                        else
                        {
                            itemDatabase[itemData.ItemID] = itemData;
                        }
                    }
                }

                yield return null; // 프레임 분산
            }

            isLoaded = true;
            Debug.Log($"[ItemManager] 총 {itemDatabase.Count}개 아이템 로드 완료!");
        }

        public void LoadAllItemsSync()
        {
            itemDatabase.Clear();

            if (loaders.Count == 0)
            {
                FindAllLoaders();
            }

            foreach (var loader in loaders)
            {
                if (loader == null) continue;

                var dataListProp = loader.GetType().GetProperty("DataList");
                if (dataListProp == null) continue;

                var dataList = dataListProp.GetValue(loader) as System.Collections.IList;
                if (dataList == null) continue;

                foreach (var staticData in dataList)
                {
                    if (staticData is LUPItemStaticData itemStaticData)
                    {
                        var itemData = itemStaticData.ToItemData();

                        if (itemDatabase.ContainsKey(itemData.ItemID))
                        {
                            itemDatabase[itemData.ItemID].MergeWith(itemData);
                        }
                        else
                        {
                            itemDatabase[itemData.ItemID] = itemData;
                        }
                    }
                }
            }

            isLoaded = true;
            Debug.Log($"[ItemManager] {itemDatabase.Count}개 아이템 로드 완료!");
        }

        public IItemable GetItem(int itemID)
        {
            if (!isLoaded)
            {
                Debug.LogWarning("[ItemManager] 아이템이 로드되지 않았습니다. LoadAllItems()를 먼저 호출하세요.");
                return null;
            }

            if (itemID == 0)
            {
                Debug.LogWarning("[ItemManager] ItemID가 유효하지 않습니다.");
                return null;
            }

            if (itemDatabase.TryGetValue(itemID, out LUPItemData item))
            {
                return item;
            }

            Debug.LogWarning($"[ItemManager] 아이템을 찾을 수 없습니다: {itemID}");
            return null;
        }

        public IItemable GetItem(string itemName)
        {
            if (!isLoaded) return null;

            if (string.IsNullOrEmpty(itemName))
            {
                Debug.LogWarning("[ItemManager] 아이템 이름이 유효하지 않습니다.");
                return null;
            }

            foreach (var item in itemDatabase.Values)
            {
                if (item.ItemName == itemName)
                {
                    return item;
                }
            }

            Debug.LogWarning($"[ItemManager] 아이템을 찾을 수 없습니다: {itemName}");
            return null;
        }

        public List<LUPItemData> GetItemsByType(Define.ItemType type)
        {
            var result = new List<LUPItemData>();
            foreach (var item in itemDatabase.Values)
            {
                if (item.Type == type)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public bool HasItem(int itemID)
        {
            return itemDatabase.ContainsKey(itemID);
        }

        public IEnumerable<LUPItemData> GetAllItems()
        {
            return itemDatabase.Values;
        }

        public int GetItemCount()
        {
            return itemDatabase.Count;
        }

        public bool IsLoaded => isLoaded;

        public void AddLoader(BaseStaticDataLoader loader)
        {
            if (loader != null && !loaders.Contains(loader))
            {
                loaders.Add(loader);
            }
        }
    }
}
