using UnityEngine;
using UnityEngine.UI;

namespace LUP.ES
{
    public class InventorySlotUI : MonoBehaviour
    {
        public Image iconImgae;
        public Text stackText;
        [HideInInspector]
        public Button slotButton;

        private int slotIndex;
        private InventoryUIController inventoryUIController;
        private ItemIconLoader itemIconLoader;

        public void Init(int slotIndex, InventoryUIController inventoryUIController, ItemIconLoader itemIconLoader)
        {
            this.slotIndex = slotIndex;
            this.inventoryUIController = inventoryUIController;
            this.itemIconLoader = itemIconLoader;

            if (slotButton == null) slotButton = GetComponent<Button>();

            slotButton.onClick.RemoveAllListeners(); // 중복 연결 방지
            slotButton.onClick.AddListener(() => OnClickSlot());
        }

        private void OnClickSlot()
        {
            if (inventoryUIController != null)
            {
                if (slotIndex == -1)
                    return;
                inventoryUIController.OnInventorySlotClicked(slotIndex);
            }
        }

        public void UpdateSlot(InventorySlot dataSlot)
        {
            if (dataSlot.IsEmpty)
            {
                iconImgae.gameObject.SetActive(false);
                stackText.text = "";
            }
            else
            {
                iconImgae.gameObject.SetActive(true);
                iconImgae.sprite = itemIconLoader.LoadIconSprite(dataSlot.item.baseItem.ID);

                stackText.text = dataSlot.item.count.ToString();
            }
        }

    }
}
