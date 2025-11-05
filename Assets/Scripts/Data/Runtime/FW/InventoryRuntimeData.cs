using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 전체의 저장 데이터
/// JSON으로 직렬화하여 파일에 저장됨
/// </summary>
[Serializable]
public class InventoryRuntimeData : BaseRuntimeData
{
    [SerializeField] private int _capacity = 30;
    [SerializeField] private List<ItemRuntimeData> _slots = new List<ItemRuntimeData>();

    public int Capacity
    {
        get => _capacity;
        set => SetValue(ref _capacity, value);
    }

    public List<ItemRuntimeData> Slots
    {
        get => _slots;
        set => SetValue(ref _slots, value);
    }

    public override void ResetData()
    {
        _capacity = 30;
        _slots.Clear();
    }

    /// <summary>
    /// 빈 슬롯으로 초기화
    /// </summary>
    public void InitializeEmptySlots(int capacity)
    {
        _capacity = capacity;
        _slots.Clear();

        for (int i = 0; i < capacity; i++)
        {
            _slots.Add(new ItemRuntimeData
            {
                ItemId = -1,
                Count = 0
            });
        }
    }

    /// <summary>
    /// 특정 슬롯에 아이템 저장
    /// </summary>
    public void SetSlot(int index, ItemRuntimeData itemData)
    {
        if (index < 0 || index >= _slots.Count)
        {
            Debug.LogError($"Invalid slot index: {index}");
            return;
        }

        _slots[index] = itemData;
    }

    /// <summary>
    /// 특정 슬롯 데이터 가져오기
    /// </summary>
    public ItemRuntimeData GetSlot(int index)
    {
        if (index < 0 || index >= _slots.Count)
        {
            Debug.LogError($"Invalid slot index: {index}");
            return null;
        }

        return _slots[index];
    }
}
