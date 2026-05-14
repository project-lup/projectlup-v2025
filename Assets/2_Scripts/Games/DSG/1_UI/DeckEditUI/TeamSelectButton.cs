using System;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class TeamSelectButton : MonoBehaviour
    {
        [SerializeField]
        private Toggle toggle;

        public int teamIndex;
        public event Action<int> OnTeamSelected;

        private void Awake()
        {
            if (toggle == null) toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }

        public void ButtonStateChange(bool isOn)
        {
            if (toggle == null) return;

            toggle.SetIsOnWithoutNotify(isOn);
            UpdateVisual(isOn);
        }

        private void OnToggleChanged(bool isOn)
        {
            UpdateVisual(isOn);

            if (!isOn) return;

            OnTeamSelected?.Invoke(teamIndex);
            SoundManager.Instance.PlaySFX("Inventory Stash 2");
        }

        private void UpdateVisual(bool isOn)
        {
            toggle.image.color = isOn ? Color.gray : Color.white;
        }
    }
}