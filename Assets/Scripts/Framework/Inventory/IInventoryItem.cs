using UnityEngine;

/// <summary>
/// 인벤토리 아이템의 기본 인터페이스
/// 모든 아이템은 이 인터페이스를 구현해야 함
/// </summary>
public interface IInventoryItem
{
    /// <summary>아이템 고유 ID (데이터베이스 키)</summary>
    int ItemID { get; }

    /// <summary>아이템 이름</summary>
    string ItemName { get; }

    /// <summary>아이템 설명</summary>
    string Description { get; }

    /// <summary>아이템 아이콘</summary>
    Sprite Icon { get; }

    /// <summary>아이템 타입 (무기, 방어구, 소비 등)</summary>
    Define.ItemType ItemType { get; }

    /// <summary>최대 스택 크기</summary>
    int MaxStackSize { get; }

    /// <summary>스택 가능 여부</summary>
    bool IsStackable { get; }

    /// <summary>아이템 사용 가능 여부 (게임별 구현)</summary>
    bool CanUse();

    /// <summary>저장용 데이터로 변환</summary>
    ItemRuntimeData ToSaveData();

    /// <summary>저장된 데이터에서 복원</summary>
    void FromSaveData(ItemRuntimeData saveData);
}
