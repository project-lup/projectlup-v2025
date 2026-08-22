using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class LobbyInventoryUIController : MonoBehaviour
    {
        [HideInInspector]
        public LobbyInventoryCenter lobbyInventoryCenter;

        public GameObject itemSlotPrefab;
        public Transform meleeSlotsParent;
        public Transform rangedSlotsParent;
        public Transform throwingSlotsParent;
        public ItemIconLoader itemIconLoader;

        private List<LobbyInventorySlotUI> meleeUiSlots = new List<LobbyInventorySlotUI>();
        private List<LobbyInventorySlotUI> rangedUiSlots = new List<LobbyInventorySlotUI>();
        private List<LobbyInventorySlotUI> throwingUiSlots = new List<LobbyInventorySlotUI>();

        public LobbyInventorySlotUI meleeWeaponSlot;
        public LobbyInventorySlotUI rangedWeaponSlot;
        public LobbyInventorySlotUI throwingWeaponSlot;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            lobbyInventoryCenter = FindAnyObjectByType<LobbyInventoryCenter>();
            if (lobbyInventoryCenter != null)
            {
                lobbyInventoryCenter.OnMeleePlayerInventoryUpdated += MeleeUpdateUI;
                lobbyInventoryCenter.OnRangedPlayerInventoryUpdated += RangedUpdateUI;
                lobbyInventoryCenter.OnthrowingPlayerInventoryUpdated += ThrowingUpdateUI;
                lobbyInventoryCenter.OnLobbyInventoryUIControllerInit += InitUI;
            }
        }

        private void OnDestroy()
        {
            if (lobbyInventoryCenter != null)
            {
                lobbyInventoryCenter.OnMeleePlayerInventoryUpdated -= MeleeUpdateUI;
                lobbyInventoryCenter.OnRangedPlayerInventoryUpdated -= RangedUpdateUI;
                lobbyInventoryCenter.OnthrowingPlayerInventoryUpdated -= ThrowingUpdateUI;
                lobbyInventoryCenter.OnLobbyInventoryUIControllerInit -= InitUI;
            }
        }

        public void InitUI()
        {
            for (int i = 0; i < lobbyInventoryCenter.meleePlayerInventory.Count; i++)
            {
                GameObject uiObject = Instantiate(itemSlotPrefab, meleeSlotsParent);
                LobbyInventorySlotUI uiSlot = uiObject.GetComponent<LobbyInventorySlotUI>();

                uiSlot.Init(i, this, itemIconLoader, WeaponType.Melee);
                meleeUiSlots.Add(uiSlot);
            }
            for (int i = 0; i < lobbyInventoryCenter.rangedPlayerInventory.Count; i++)
            {
                GameObject uiObject = Instantiate(itemSlotPrefab, rangedSlotsParent);
                LobbyInventorySlotUI uiSlot = uiObject.GetComponent<LobbyInventorySlotUI>();

                uiSlot.Init(i, this, itemIconLoader, WeaponType.Ranged);
                rangedUiSlots.Add(uiSlot);
            }
            for (int i = 0; i < lobbyInventoryCenter.throwingPlayerInventory.Count; i++)
            {
                GameObject uiObject = Instantiate(itemSlotPrefab, throwingSlotsParent);
                LobbyInventorySlotUI uiSlot = uiObject.GetComponent<LobbyInventorySlotUI>();

                uiSlot.Init(i, this, itemIconLoader, WeaponType.Throwing);
                throwingUiSlots.Add(uiSlot);
            }
            meleeWeaponSlot.Init(-1, this, itemIconLoader, WeaponType.Melee);
            rangedWeaponSlot.Init(-1, this, itemIconLoader, WeaponType.Ranged);
            throwingWeaponSlot.Init(-1, this, itemIconLoader, WeaponType.Throwing);

            MeleeUpdateUI();
            RangedUpdateUI();
            ThrowingUpdateUI();
        }

        public void OnWeaponSlotClicked(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Melee:
                    foreach (InventorySlot slot in lobbyInventoryCenter.meleePlayerInventory)
                    {
                        if (slot.IsEmpty)
                        {
                            lobbyInventoryCenter.UnEquip(slot, weaponType);
                            break;
                        }
                    }
                    break;
                case WeaponType.Ranged:
                    foreach (InventorySlot slot in lobbyInventoryCenter.rangedPlayerInventory)
                    {
                        if (slot.IsEmpty)
                        {
                            lobbyInventoryCenter.UnEquip(slot, weaponType);
                            break;
                        }
                    }
                    break;
                case WeaponType.Throwing:
                    foreach (InventorySlot slot in lobbyInventoryCenter.throwingPlayerInventory)
                    {
                        if (slot.IsEmpty)
                        {
                            lobbyInventoryCenter.UnEquip(slot, weaponType);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void OnMeleeInventorySlotClicked(int slotIndex)
        {
            // 1. 유효성 검사
            if (slotIndex < 0 || slotIndex >= lobbyInventoryCenter.meleePlayerInventory.Count) return;

            InventorySlot slotData = lobbyInventoryCenter.meleePlayerInventory[slotIndex];

            // 2. 빈 슬롯이면 아무것도 안 함
            if (slotData.IsEmpty) return;

            Debug.Log($"슬롯 클릭됨: {slotIndex}번, 아이템: {slotData.item.baseItem.Name}");

            // 3. 인벤토리에게 장착 요청
            lobbyInventoryCenter.EquipItem(slotIndex, WeaponType.Melee);
        }

        public void OnRangedInventorySlotClicked(int slotIndex)
        {
            // 1. 유효성 검사
            if (slotIndex < 0 || slotIndex >= lobbyInventoryCenter.rangedPlayerInventory.Count) return;

            InventorySlot slotData = lobbyInventoryCenter.rangedPlayerInventory[slotIndex];

            // 2. 빈 슬롯이면 아무것도 안 함
            if (slotData.IsEmpty) return;

            Debug.Log($"슬롯 클릭됨: {slotIndex}번, 아이템: {slotData.item.baseItem.Name}");

            // 3. 인벤토리에게 장착 요청
            lobbyInventoryCenter.EquipItem(slotIndex, WeaponType.Ranged);
        }

        public void OnThrowingInventorySlotClicked(int slotIndex)
        {
            // 1. 유효성 검사
            if (slotIndex < 0 || slotIndex >= lobbyInventoryCenter.throwingPlayerInventory.Count) return;

            InventorySlot slotData = lobbyInventoryCenter.throwingPlayerInventory[slotIndex];

            // 2. 빈 슬롯이면 아무것도 안 함
            if (slotData.IsEmpty) return;

            Debug.Log($"슬롯 클릭됨: {slotIndex}번, 아이템: {slotData.item.baseItem.Name}");

            // 3. 인벤토리에게 장착 요청
            lobbyInventoryCenter.EquipItem(slotIndex, WeaponType.Throwing);
        }


        public void MeleeUpdateUI()
        {
            for (int i = 0; i < meleeUiSlots.Count; i++)
            {
                InventorySlot dataSlot = lobbyInventoryCenter.meleePlayerInventory[i];
                meleeUiSlots[i].UpdateSlot(dataSlot);
            }
            meleeWeaponSlot.UpdateSlot(lobbyInventoryCenter.meleeWeaponSlot);
        }
        public void RangedUpdateUI()
        {
            for (int i = 0; i < rangedUiSlots.Count; i++)
            {
                InventorySlot dataSlot = lobbyInventoryCenter.rangedPlayerInventory[i];
                rangedUiSlots[i].UpdateSlot(dataSlot);
            }
            rangedWeaponSlot.UpdateSlot(lobbyInventoryCenter.rangedWeaponSlot);
        }

        public void ThrowingUpdateUI()
        {
            for (int i = 0; i < throwingUiSlots.Count; i++)
            {
                InventorySlot dataSlot = lobbyInventoryCenter.throwingPlayerInventory[i];
                throwingUiSlots[i].UpdateSlot(dataSlot);
            }
            throwingWeaponSlot.UpdateSlot(lobbyInventoryCenter.throwingWeaponSlot);
        }

    }
}