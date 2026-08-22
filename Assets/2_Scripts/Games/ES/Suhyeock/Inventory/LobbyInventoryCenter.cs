using LUP;
using LUP.RL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class LobbyInventoryCenter : MonoBehaviour
    {
        ExtractionShooterStage extractionShooterStage;
        public ItemDataBase itemDataBase;
        public event Action OnMeleePlayerInventoryUpdated;
        public event Action OnRangedPlayerInventoryUpdated;
        public event Action OnthrowingPlayerInventoryUpdated;
        public event Action OnLobbyInventoryUIControllerInit;

        public int inventorySlotSize = 50;
        [HideInInspector]
        public List<InventorySlot> meleePlayerInventory = new List<InventorySlot>();
        [HideInInspector]
        public List<InventorySlot> rangedPlayerInventory = new List<InventorySlot>();
        [HideInInspector] 
        public List<InventorySlot> throwingPlayerInventory = new List<InventorySlot>();

        public InventorySlot meleeWeaponSlot;
        public InventorySlot rangedWeaponSlot;
        public InventorySlot throwingWeaponSlot;

        public void Init()
        {
            for (int i = 0; i < inventorySlotSize; i++)
            {
                meleePlayerInventory.Add(new InventorySlot());
                rangedPlayerInventory.Add(new InventorySlot());
                throwingPlayerInventory.Add(new InventorySlot());
            }

            extractionShooterStage = StageManager.Instance.GetCurrentStage() as ExtractionShooterStage;
            List<LUP.InventorySlot> allSlots = extractionShooterStage.ESInven.GetAllItems();
            int meleeSlotCount = 0;
            int rangedSlotCount = 0;
            int throwingSlotCount = 0;
            foreach (LUP.InventorySlot slot in allSlots)
            {
                BaseItemData item = itemDataBase.GetItemByID(slot.Item.ItemID);
                if (item == null)
                    continue;
                
                if (item.itemType == ItemType.Weapon)
                {
                    WeaponItemData weaponItemData = item as WeaponItemData;

                    if (weaponItemData == null)
                        continue;
                    WeaponItem weaponItem = new WeaponItem(weaponItemData);
                    switch (weaponItemData.weaponType)
                    {
                        case WeaponType.Melee:
                            meleePlayerInventory[meleeSlotCount++].item = weaponItem;
                            break;
                        case WeaponType.Ranged:
                            rangedPlayerInventory[rangedSlotCount++].item = weaponItem;
                            break;
                        case WeaponType.Throwing:
                            throwingPlayerInventory[throwingSlotCount++].item = weaponItem;
                            break;
                        default:
                            break;
                    }
                    
                }
            }
            meleeWeaponSlot = new InventorySlot();
            rangedWeaponSlot = new InventorySlot();
            throwingWeaponSlot = new InventorySlot();
            OnLobbyInventoryUIControllerInit?.Invoke();
            OnMeleePlayerInventoryUpdated?.Invoke();
            OnRangedPlayerInventoryUpdated?.Invoke();
            OnthrowingPlayerInventoryUpdated?.Invoke();
        }

        public void EquipItem(int slotIndex, WeaponType weaponType)
        {
            InventorySlot slot = null;
            switch (weaponType)
            {
                case WeaponType.Melee:
                    slot = meleePlayerInventory[slotIndex];
                    break;
                case WeaponType.Ranged:
                    slot = rangedPlayerInventory[slotIndex];
                    break;
                case WeaponType.Throwing:
                    slot = throwingPlayerInventory[slotIndex];
                    break;
                default:
                    break;
            }

            if (slot.IsEmpty) return;

            Item itemToEquip = slot.item;

            if (itemToEquip == null)
                return;

            if (itemToEquip.baseItem.itemType == ItemType.Material ||
                itemToEquip.baseItem.itemType == ItemType.Consumable)
                return;

            Item previousItem = null;

            switch (weaponType)
            {
                case WeaponType.Melee:
                    previousItem = meleeWeaponSlot.item;
                    meleeWeaponSlot.item = slot.item;
                    slot.item = previousItem;
                    OnMeleePlayerInventoryUpdated?.Invoke();
                    break;
                case WeaponType.Ranged:
                    previousItem = rangedWeaponSlot.item;
                    rangedWeaponSlot.item = slot.item;
                    slot.item = previousItem;
                    OnRangedPlayerInventoryUpdated?.Invoke();
                    break;
                case WeaponType.Throwing:
                    previousItem = throwingWeaponSlot.item;
                    throwingWeaponSlot.item = slot.item;
                    slot.item = previousItem;
                    OnthrowingPlayerInventoryUpdated?.Invoke();
                    break;
                default:
                    break;
            }        
        }

        public void UnEquip(InventorySlot slot, WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Melee:
                    slot.item = meleeWeaponSlot.item;
                    meleeWeaponSlot.item = null;
                    OnMeleePlayerInventoryUpdated?.Invoke();
                    break;
                case WeaponType.Ranged:
                    slot.item = rangedWeaponSlot.item;
                    rangedWeaponSlot.item = null;
                    OnRangedPlayerInventoryUpdated?.Invoke();
                    break;
                case WeaponType.Throwing:
                    slot.item = throwingWeaponSlot.item;
                    throwingWeaponSlot.item = null;
                    OnthrowingPlayerInventoryUpdated?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
}

