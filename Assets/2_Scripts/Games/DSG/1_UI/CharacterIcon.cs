using LUP.DSG.Utils.Enums;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class CharacterIcon : MonoBehaviour
    {
        [SerializeField]
        private Image portrait;
        [SerializeField]
        private Image attributeIcon;
        [SerializeField]
        private TextMeshProUGUI level;
        [SerializeField]
        private float iconWidth = 400f;
        [SerializeField]
        private float iconHeight = 800f;

        public CharacterSelectButton selectedButton;

        private int characterId;
        private Action<int, CharacterSelectButton> OnSelected;
        private Action<int, CharacterSelectButton> OnDeselected;
        private Action<int, bool> OnCheckedChanged;

        private void OnEnable()
        {
            IconBootstrapper.OnAllIconsGenerated += RefreshIcon;
        }

        private void OnDisable()
        {
            IconBootstrapper.OnAllIconsGenerated -= RefreshIcon;
        }
        public void Init(
            Action<int, CharacterSelectButton> onSelected,
            Action<int, CharacterSelectButton> onDeselected,
            Action<int, bool> onCheckedChanged)
        {
            OnSelected = onSelected;
            OnDeselected = onDeselected;
            OnCheckedChanged = onCheckedChanged;

            selectedButton.Init();
            selectedButton.button.onClick.AddListener(OnButtonClicked);

            SetIconRectSize(iconWidth, iconHeight);
        }

        public void SetIconData(int id, int characterLevel, AttributeTypeImage typeIcon)
        {
            characterId = id;
            level.text = "Lv." + characterLevel;

            if (CharacterIconCache.TryGetByCharacterId(characterId, out var sprite))
            {
                portrait.sprite = sprite;
                portrait.color = Color.white;
            }
            else
            {
                // 아직 안 만들어졌으면 일단 색만 입힘
                portrait.sprite = null;
            }

            attributeIcon.sprite = typeIcon.typeIcon;
            attributeIcon.color = typeIcon.typeColor;
        }

        public void OnButtonClicked()
        {
            if (selectedButton.isSelected)
                OnDeselected?.Invoke(characterId, selectedButton);
            else
                OnSelected?.Invoke(characterId, selectedButton);

            OnCheckedChanged?.Invoke(characterId, selectedButton.isSelected);
        }
        private void RefreshIcon()
        {
            if (CharacterIconCache.TryGetByCharacterId(characterId, out var sprite))
            {
                portrait.sprite = sprite;
                portrait.color = Color.white;
                portrait.preserveAspect = true;
                
            }
        }

        public void SetIconRectSize(float width, float height)
        {
            RectTransform rt = portrait.rectTransform;

            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

    }
}