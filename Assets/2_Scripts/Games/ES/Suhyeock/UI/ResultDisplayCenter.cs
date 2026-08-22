using DG.Tweening;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using static UnityEditor.Progress;
using TMPro;

namespace LUP.ES
{
    public class ResultDisplayCenter : MonoBehaviour
    {
        private EventBroker eventBroker;
        private ItemCenter itemCenter; //테스트 용
        public GameObject resultPanel;
        public GameObject ItemDisplayContent;
        public GameObject itemSlotPrefab;
        public TextMeshProUGUI resultHeader;    
        //public Button lobbyButton;

        private Transform contentParent;
        private List<Item> items;

        private List<GameObject> slotPool = new List<GameObject>();
        [Header("풀링 설정")]
        public int initialSlotCount = 30;
        private void Start()
        {
            resultPanel.SetActive(false);
            //lobbyButton.onClick.AddListener(LoadLobby);
            eventBroker = FindAnyObjectByType<EventBroker>();
            itemCenter = FindAnyObjectByType<ItemCenter>();
            contentParent = ItemDisplayContent.transform;

            PreGenerateSlots();

            if (eventBroker != null )
            {
                eventBroker.OnGameFinished += ShowResult;
            }
        }

        private void PreGenerateSlots()
        {
            for (int i = 0; i < initialSlotCount; i++)
            {
                GameObject newSlot = Instantiate(itemSlotPrefab, contentParent);
                newSlot.SetActive(false); // 일단 숨겨둠
                slotPool.Add(newSlot);
            }
        }

        private void OnDestroy()
        {
            if (eventBroker != null)
            {
                eventBroker.OnGameFinished -= ShowResult;
            }
        }

        public void ShowInventoryItems(List<Item> items)
        {
            List<GameObject> activeSlots = new List<GameObject>();

            for (int i = 0; i < items.Count; i++)
            {
                if (i >= slotPool.Count)
                {
                    GameObject newSlot = Instantiate(itemSlotPrefab, contentParent);
                    slotPool.Add(newSlot);
                }

                GameObject slotObj = slotPool[i];
                slotObj.SetActive(true);
                activeSlots.Add(slotObj);

                ItemDisplaySlot slot = slotObj.GetComponent<ItemDisplaySlot>();
                if (slot != null)
                {
                    slot.SetItem(items[i], items);
                }

                CanvasGroup canvasGroup = slotObj.GetComponent<CanvasGroup>();
                if (canvasGroup == null) canvasGroup = slotObj.AddComponent<CanvasGroup>();
                canvasGroup.alpha = 0f;

                RectTransform rect = slotObj.GetComponent<RectTransform>();
                rect.localScale = Vector3.one;
            }

            for (int i = items.Count; i < slotPool.Count; i++)
            {
                slotPool[i].SetActive(false);
            }

            Canvas.ForceUpdateCanvases();

            LayoutGroup layoutGroup = contentParent.GetComponent<LayoutGroup>();
            if (layoutGroup != null)
            {
                layoutGroup.enabled = false;
            }

            for (int i = 0; i < activeSlots.Count; i++)
            {
                GameObject slotObj = activeSlots[i];
                RectTransform rect = slotObj.GetComponent<RectTransform>();
                CanvasGroup canvasGroup = slotObj.GetComponent<CanvasGroup>();

                Vector2 originalPos = rect.anchoredPosition;
                rect.anchoredPosition = new Vector2(originalPos.x - 200f, originalPos.y);

                float delay = i * 0.1f;

                var moveTween = rect.DOAnchorPos(originalPos, 0.8f)
                    .SetDelay(delay)
                    .SetEase(Ease.OutCubic)
                    .SetUpdate(true);

                if (i == activeSlots.Count - 1)
                {
                    moveTween.OnComplete(() =>
                    {
                        if (layoutGroup != null) layoutGroup.enabled = true;
                    });
                }

                canvasGroup.DOFade(1f, 0.8f)
                    .SetDelay(delay)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true);
            }
        }

        private void ShowResult(bool isSuccess)
        {
            Debug.Log("GameFinish");
            Time.timeScale = 0f;
            StringBuilder resultHeadrString = new StringBuilder();
            resultHeadrString.Append("Extraction ");
            resultPanel.SetActive(true);

            if (isSuccess)
            {
                SoundManager.Instance.PlaySFX("SuccessfulEscape");
                resultHeadrString.Append("Complete");
                resultHeader.color = Color.white;
                Inventory inventory = FindAnyObjectByType<Inventory>();

                if (inventory != null)
                {
                    items = inventory.GetItems();
                    ExtractionShooterStage extractionShooterStage = StageManager.Instance.GetCurrentStage() as ExtractionShooterStage;

                    foreach (Item item in items)
                    {
                        if (item == null || item.ItemID == 1 || item.ItemID == 4 || item.ItemID == 7)
                            continue;
                        extractionShooterStage.ESInven.AddItem(item);
                    }

                    ShowInventoryItems(items);
                }
            }
            else
            {
                SoundManager.Instance.PlaySFX("Escape failed");
                resultHeadrString.Append("Failed");
                resultHeader.color = Color.red;
            }
            resultHeader.text = resultHeadrString.ToString();
        }
    }
}
