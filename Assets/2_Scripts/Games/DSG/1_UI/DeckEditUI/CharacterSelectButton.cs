using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class CharacterSelectButton : MonoBehaviour
    {
        public Button button { get; private set; }
        [SerializeField]
        private CanvasGroup canvasGroup;

        public bool isSelected = false;

        public void Init()
        {
            button = GetComponent<Button>();

            SetSelected(false);
        }

        public void ButtonClicked() => SetSelected(!isSelected);

        public void SetSelected(bool selected)
        {
            isSelected = selected;

            if (canvasGroup != null)
                canvasGroup.alpha = selected ? 1f : 0f;
        }
    }
}