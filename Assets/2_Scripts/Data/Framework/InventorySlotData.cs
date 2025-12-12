using System;
using UnityEngine;

[Serializable]
public class InventorySlotData
{
    [SerializeField] public string slotKey;
    [SerializeField] public int itemID;
    [SerializeField] public int quantity;
}
