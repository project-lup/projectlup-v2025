using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventory<T> : MonoBehaviour where T : class, IInventoryItem
{
    [Header("인벤토리 설정")]
    [SerializeField] protected int capacity = 30;
    [SerializeField] protected bool shouldPersist = false; // 스테이지 전환 시 유지 여부

    protected List<InventorySlot<T>> slots;

    // 이벤트
    public event Action<T, int> OnItemAdded;
    public event Action<T, int> OnItemRemoved;
    public event Action OnInventoryUpdated;
    public event Action<int> OnSlotChanged; // 특정 슬롯 변경

    // 프로퍼티
    public int Capacity => capacity;
    public bool ShouldPersist => shouldPersist;
    public List<InventorySlot<T>> Slots => slots;

    /// <summary>
    /// 초기화
    /// </summary>
    protected virtual void Awake()
    {
        InitializeSlots();
        RegisterToManager();
    }

    /// <summary>
    /// 슬롯 초기화
    /// </summary>
    protected virtual void InitializeSlots()
    {
        slots = new List<InventorySlot<T>>(capacity);
        for (int i = 0; i < capacity; i++)
        {
            slots.Add(new InventorySlot<T>(i));
        }

        Debug.Log($"[{GetType().Name}] Initialized with {capacity} slots");
    }

    /// <summary>
    /// InventoryManager에 자신을 등록
    /// </summary>
    protected virtual void RegisterToManager()
    {
        if (Manager.InventoryManager.Instance != null)
        {
            Manager.InventoryManager.Instance.RegisterInventory(this);
        }
    }

    #region 아이템 추가/제거

    /// <summary>
    /// 아이템 추가 (자동 스택 처리)
    /// </summary>
    public virtual bool AddItem(T item, int amount = 1)
    {
        if (item == null || amount <= 0)
        {
            Debug.LogWarning("Invalid item or amount");
            return false;
        }

        int remainingAmount = amount;

        // 1. 스택 가능한 경우 기존 슬롯에 추가 시도
        if (item.IsStackable)
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.Item.ItemID == item.ItemID && !slot.IsFull)
                {
                    int added = slot.AddItem(item, remainingAmount);
                    remainingAmount -= added;

                    OnSlotChanged?.Invoke(slot.SlotIndex);

                    if (remainingAmount <= 0)
                    {
                        OnItemAdded?.Invoke(item, amount);
                        OnInventoryUpdated?.Invoke();
                        BroadcastItemAdded(item, amount);
                        return true;
                    }
                }
            }
        }

        // 2. 빈 슬롯 찾기
        while (remainingAmount > 0)
        {
            InventorySlot<T> emptySlot = FindEmptySlot();
            if (emptySlot == null)
            {
                Debug.LogWarning($"Inventory full! Could not add {remainingAmount} of {item.ItemName}");

                // 부분 성공
                if (remainingAmount < amount)
                {
                    int addedAmount = amount - remainingAmount;
                    OnItemAdded?.Invoke(item, addedAmount);
                    OnInventoryUpdated?.Invoke();
                    BroadcastItemAdded(item, addedAmount);
                }

                return false;
            }

            int toAdd = Mathf.Min(remainingAmount, item.MaxStackSize);
            emptySlot.AddItem(item, toAdd);
            remainingAmount -= toAdd;

            OnSlotChanged?.Invoke(emptySlot.SlotIndex);
        }

        OnItemAdded?.Invoke(item, amount);
        OnInventoryUpdated?.Invoke();
        BroadcastItemAdded(item, amount);
        return true;
    }

    /// <summary>
    /// 특정 슬롯에서 아이템 제거
    /// </summary>
    public virtual bool RemoveItem(int slotIndex, int amount = 1)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError($"Invalid slot index: {slotIndex}");
            return false;
        }

        var slot = slots[slotIndex];
        if (slot.IsEmpty)
        {
            Debug.LogWarning($"Slot {slotIndex} is empty");
            return false;
        }

        T item = slot.Item;
        int removed = slot.RemoveItem(amount);

        if (removed > 0)
        {
            OnSlotChanged?.Invoke(slotIndex);
            OnItemRemoved?.Invoke(item, removed);
            OnInventoryUpdated?.Invoke();
            BroadcastItemRemoved(item, removed);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 아이템 ID로 제거
    /// </summary>
    public virtual bool RemoveItemByID(int itemID, int amount = 1)
    {
        int remainingAmount = amount;

        for (int i = 0; i < slots.Count && remainingAmount > 0; i++)
        {
            var slot = slots[i];
            if (!slot.IsEmpty && slot.Item.ItemID == itemID)
            {
                int toRemove = Mathf.Min(remainingAmount, slot.Count);
                RemoveItem(i, toRemove);
                remainingAmount -= toRemove;
            }
        }

        return remainingAmount == 0;
    }

    /// <summary>
    /// 특정 슬롯 비우기
    /// </summary>
    public virtual void ClearSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError($"Invalid slot index: {slotIndex}");
            return;
        }

        var slot = slots[slotIndex];
        if (!slot.IsEmpty)
        {
            T item = slot.Item;
            int count = slot.Count;
            slot.Clear();

            OnSlotChanged?.Invoke(slotIndex);
            OnItemRemoved?.Invoke(item, count);
            OnInventoryUpdated?.Invoke();
            BroadcastItemRemoved(item, count);
        }
    }

    /// <summary>
    /// 전체 인벤토리 비우기
    /// </summary>
    public virtual void ClearAll()
    {
        foreach (var slot in slots)
        {
            slot.Clear();
        }

        OnInventoryUpdated?.Invoke();
        Debug.Log($"[{GetType().Name}] Inventory cleared");
    }

    #endregion

    #region 조회 메서드

    /// <summary>
    /// 빈 슬롯 찾기
    /// </summary>
    public virtual InventorySlot<T> FindEmptySlot()
    {
        return slots.Find(s => s.IsEmpty);
    }

    /// <summary>
    /// 아이템 ID로 슬롯 찾기
    /// </summary>
    public virtual InventorySlot<T> FindSlotByItemID(int itemID)
    {
        return slots.Find(s => !s.IsEmpty && s.Item.ItemID == itemID);
    }

    /// <summary>
    /// 빈 슬롯 개수
    /// </summary>
    public virtual int GetEmptySlotCount()
    {
        return slots.FindAll(s => s.IsEmpty).Count;
    }

    /// <summary>
    /// 인벤토리에 공간이 있는지 확인
    /// </summary>
    public virtual bool HasSpace()
    {
        return FindEmptySlot() != null;
    }

    /// <summary>
    /// 특정 아이템을 보유하고 있는지 확인
    /// </summary>
    public virtual bool HasItem(int itemID, int amount = 1)
    {
        int totalCount = 0;

        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.Item.ItemID == itemID)
            {
                totalCount += slot.Count;
                if (totalCount >= amount)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 특정 아이템의 총 개수
    /// </summary>
    public virtual int GetItemCount(int itemID)
    {
        int count = 0;

        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.Item.ItemID == itemID)
            {
                count += slot.Count;
            }
        }

        return count;
    }

    /// <summary>
    /// 특정 슬롯 가져오기
    /// </summary>
    public virtual InventorySlot<T> GetSlot(int index)
    {
        if (index < 0 || index >= slots.Count)
        {
            Debug.LogError($"Invalid slot index: {index}");
            return null;
        }

        return slots[index];
    }

    #endregion

    #region 슬롯 조작

    /// <summary>
    /// 두 슬롯 교환
    /// </summary>
    public virtual void SwapSlots(int slotIndexA, int slotIndexB)
    {
        if (slotIndexA < 0 || slotIndexA >= slots.Count ||
            slotIndexB < 0 || slotIndexB >= slots.Count)
        {
            Debug.LogError("Invalid slot indices for swap");
            return;
        }

        slots[slotIndexA].SwapWith(slots[slotIndexB]);

        OnSlotChanged?.Invoke(slotIndexA);
        OnSlotChanged?.Invoke(slotIndexB);
        OnInventoryUpdated?.Invoke();
    }

    #endregion

    #region 저장/로드

    /// <summary>
    /// 저장용 데이터로 변환 (각 게임에서 구현)
    /// </summary>
    public abstract InventoryRuntimeData ToSaveData();

    /// <summary>
    /// 저장된 데이터에서 복원 (각 게임에서 구현)
    /// </summary>
    public abstract void FromSaveData(InventoryRuntimeData saveData);

    #endregion

    #region Manager 연동

    /// <summary>
    /// InventoryManager로 이벤트 브로드캐스트
    /// </summary>
    protected virtual void BroadcastItemAdded(T item, int amount)
    {
        if (Manager.InventoryManager.Instance != null)
        {
            Manager.InventoryManager.Instance.BroadcastItemAdded(item, amount);
        }
    }

    protected virtual void BroadcastItemRemoved(T item, int amount)
    {
        if (Manager.InventoryManager.Instance != null)
        {
            Manager.InventoryManager.Instance.BroadcastItemRemoved(item, amount);
        }
    }

    #endregion

    #region 디버깅

    /// <summary>
    /// 인벤토리 상태 출력 (디버그용)
    /// </summary>
    public virtual void DebugPrintInventory()
    {
        Debug.Log($"===== {GetType().Name} =====");
        Debug.Log($"Capacity: {capacity}, Empty Slots: {GetEmptySlotCount()}");

        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            if (!slot.IsEmpty)
            {
                Debug.Log($"[{i}] {slot.Item.ItemName} x{slot.Count}");
            }
        }
    }

    #endregion
}
