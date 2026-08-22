using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    [CreateAssetMenu(fileName = "PrefabDataBase", menuName = "ES/PrefabDataBase")]
    public class PrefabDataBase : ScriptableObject
    {
        [System.Serializable]
        public class ItemPrefabEntry
        {
            [ReadOnly] public int id;
            [ReadOnly] public string name;
            public GameObject prefab;
            public Vector3 positionOffset;
            public Vector3 rotationOffset;
        }

        //public ItemDataBase itemDataBase;

        [Header("Source Data")]
        public ESItemStaticDataLoader itemLoader; 

        [Header("Prefab List")]
        public List<ItemPrefabEntry> prefabList = new List<ItemPrefabEntry>();


        private readonly List<string> weaponTypeStrings = new List<string>()
        {
            "Weapon",
            "RangedWeapon",
            "MeleeWeapon",
            "ThrowingWeapon" 
        };

        [ContextMenu("Sync IDs from ItemDB")]
        public void SyncIds()
        {

            if (itemLoader == null)
            {
                Debug.LogError("Item Loader가 연결되지 않았습니다! 인스펙터에서 연결해주세요.");
                return;
            }

            List<ESItemStaticData> sourceList = itemLoader.GetDataList();

            if (sourceList == null || sourceList.Count == 0)
            {
                Debug.LogError("로더에 데이터가 없습니다. 로더 에셋에서 'Load' 버튼을 먼저 눌러주세요.");
                return;

            }
            int updateCount = 0;
            int newCount = 0;
            int removeCount = 0;

            HashSet<int> validWeaponIds = new HashSet<int>();

            foreach (var staticData in sourceList)
            {
                if (IsWeapon(staticData.ItemType))
                {
                    validWeaponIds.Add(staticData.ItemID);

                    ItemPrefabEntry existingEntry = prefabList.Find(x => x.id == staticData.ItemID);

                    if (existingEntry != null)
                    {
                        if (existingEntry.name != staticData.ItemName)
                            existingEntry.name = staticData.ItemName;
                        updateCount++;
                    }
                    else
                    {
                        prefabList.Add(new ItemPrefabEntry
                        {
                            id = staticData.ItemID,
                            name = staticData.ItemName,
                            prefab = null,
                            positionOffset = Vector3.zero,
                            rotationOffset = Vector3.zero
                        });
                        newCount++;
                    }
                }
            }

            for (int i = prefabList.Count - 1; i >= 0; i--)
            {
                if (!validWeaponIds.Contains(prefabList[i].id))
                {
                    prefabList.RemoveAt(i);
                    removeCount++;
                }
            }

            Debug.Log($"[PrefabDB] 동기화 완료! (갱신: {updateCount}, 신규: {newCount}, 제거됨: {removeCount})");

            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
            
        }

        private bool IsWeapon(string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;

            foreach (var weaponType in weaponTypeStrings)
            {
                if (string.Equals(typeStr, weaponType, System.StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public GameObject GetPrefab(int id)
        {
            ItemPrefabEntry entry = prefabList.Find(x => x.id == id);
            if (entry != null) return entry.prefab;
            return null;
        }

        public ItemPrefabEntry GetEntry(int id)
        {
            ItemPrefabEntry entry = prefabList.Find(x => x.id == id);
            return entry;
        }
    }

}
