using UnityEngine;
using UnityEngine.UI;

namespace LUP.ES
{
    public class InteractionUIController : MonoBehaviour
    {
        private GameObject InteractionCanvas;
        [SerializeField]
        private GameObject InteractionPromptPrefab;
        [SerializeField]
        private GameObject InteractionTimerPrefab;
        [SerializeField]
        private float YOffset = 40.0f;

        private Image InteractionTimerUIImage;
        private Image InteractionTimerImage;
        private Image InteractionPromptImage;

        private Camera MainCamera;
        private EventBroker eventBroker;
        private Transform currentTargetTransform;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            {
                InteractionCanvas = GameObject.FindWithTag("InteractionCanvas");
                MainCamera = Camera.main;
                eventBroker = FindAnyObjectByType<EventBroker>();
            }
            if (eventBroker != null)
            {
                eventBroker.OnInteractionPromptStateChanged += HandlePromptState;
                eventBroker.OnInteractionTimerUpdated += HandleTimerUpdate;
            }
            {
                InitUI();
            }
        }
        private void OnDestroy()
        {
            if (eventBroker != null)
            {
                eventBroker.OnInteractionPromptStateChanged -= HandlePromptState;
                eventBroker.OnInteractionTimerUpdated -= HandleTimerUpdate;
            }
        }

        void InitUI()
        {
            GameObject UIInstance1 = Instantiate(InteractionPromptPrefab, InteractionCanvas.transform);
            InteractionPromptImage = UIInstance1.GetComponent<Image>();
            InteractionPromptImage.gameObject.SetActive(false);

            GameObject UIInstance2 = Instantiate(InteractionTimerPrefab, InteractionCanvas.transform);
            InteractionTimerUIImage = UIInstance2.GetComponent<Image>();

            Transform child = UIInstance2.transform.Find("InteractionTimer");
            InteractionTimerImage = child.GetComponent<Image>();
            InteractionTimerImage.fillAmount = 1;
            InteractionTimerUIImage.gameObject.SetActive(false);

        }

        private void HandlePromptState(bool isVisible, Transform targetTransform)
        {
            if (isVisible)
            {
                currentTargetTransform = targetTransform;
                InteractionPromptImage.gameObject.SetActive(true);
            }
            else
            {
                currentTargetTransform = null;
                InteractionPromptImage.gameObject.SetActive(false);
                InteractionTimerUIImage.gameObject.SetActive(false);
            }
        }

        private void HandleTimerUpdate(float currentTime, float maxTime)
        {
            if (maxTime <= 0) return;
            if (currentTime > 0 && currentTime < maxTime)
            {
                InteractionTimerUIImage.gameObject.SetActive(true);
                InteractionTimerImage.fillAmount = currentTime / maxTime;
            }
            else
                InteractionTimerUIImage.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            if (currentTargetTransform != null && InteractionPromptImage.gameObject.activeSelf)
            {
                Vector3 ScreenPostion = MainCamera.WorldToScreenPoint(currentTargetTransform.position);
                ScreenPostion.y += YOffset;

                InteractionPromptImage.rectTransform.position = ScreenPostion;
                if (InteractionTimerUIImage.gameObject.activeSelf)
                {
                    InteractionTimerUIImage.rectTransform.position = ScreenPostion;
                }
            }
        }
    }
}
