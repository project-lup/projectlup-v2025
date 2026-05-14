using LUP.DSG.Utils.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public class CharactersList : MonoBehaviour
    {
        [SerializeField]
        private GameObject iconPrefab;
        [SerializeField]
        private Transform contentParent;

        private readonly Dictionary<int, bool> selectedOwnedMap = new Dictionary<int, bool>();
        private readonly List<CharacterIcon> iconPool = new List<CharacterIcon>();

        private int activeCount = 0;

        private Action<int, CharacterSelectButton> OnIconSelected;
        private Action<int, CharacterSelectButton> OnIconDeselected;

        public void BindCallbacks(
          Action<int, CharacterSelectButton> onSelected,
          Action<int, CharacterSelectButton> onDeselected)
        {
            OnIconSelected = onSelected;
            OnIconDeselected = onDeselected;
        }

        public void ResetSelectedStatus()
        {
            selectedOwnedMap.Clear();

            for (int i = 0; i < iconPool.Count; i++)
            {
                if (iconPool[i]?.selectedButton != null)
                    iconPool[i].selectedButton.SetSelected(false);
            }
        }

        public void UpdateCharacterIcon(CharacterInfo info, AttributeTypeImage typeIcon)
        {
            if (info == null || typeIcon.typeIcon == null) return;

            CharacterIcon icon = GetOrCreateIcon();
            if (icon?.selectedButton == null) return;

            icon.transform.SetParent(contentParent, false);
            icon.gameObject.SetActive(true);
            icon.SetIconData(info.characterID, info.characterLevel, typeIcon);

            bool isSelected = selectedOwnedMap.TryGetValue(info.characterID, out bool value) && value;
            icon.selectedButton.SetSelected(isSelected);
        }

        public void UpdateCheckedList(int characterID, bool isChecked)
        {
            if (isChecked)
                selectedOwnedMap[characterID] = true;
            else
                selectedOwnedMap.Remove(characterID);
        }

        public void ReleaseAllIcons()
        {
            for (int i = 0; i < iconPool.Count; i++)
            {
                if (iconPool[i] == null) continue;

                iconPool[i].selectedButton?.SetSelected(false);
                iconPool[i].gameObject.SetActive(false);
            }

            activeCount = 0;
        }

        private CharacterIcon GetOrCreateIcon()
        {
            if (activeCount < iconPool.Count)
                return iconPool[activeCount++];

            if (iconPrefab == null || contentParent == null)
                return null;

            GameObject newIcon = Instantiate(iconPrefab, contentParent);
            CharacterIcon icon = newIcon.GetComponent<CharacterIcon>();
            if (icon == null)
            {
                Destroy(newIcon);
                return null;
            }

            icon.Init(OnIconSelected, OnIconDeselected, UpdateCheckedList);
            iconPool.Add(icon);
            activeCount++;
            return icon;
        }
    }
}