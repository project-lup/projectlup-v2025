using LUP.DSG.Utils.Enums;
using UnityEngine;

namespace LUP.DSG
{
    public class CharacterHeadupUI : MonoBehaviour
    {
        private Transform target;
        private Vector3 offset = new Vector3(0, 0, 0);

        private Camera mainCamera;
        private RectTransform rectTransform;
        private RectTransform canvasRect;

        [Header("보정 설정")]
        [Range(0f, 0.2f)]
        public float distortionFactor = 0f;

        private CanvasGroup canvasGroup;

        private CharacterInfoUI characterInfoUI;
        private CharacterBattleUI characterBattleUI;

        void Awake()
        {
            mainCamera = Camera.main;
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();

            if (transform.parent != null)
                canvasRect = transform.parent as RectTransform;

            if (canvasGroup != null)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            characterInfoUI = GetComponentInChildren<CharacterInfoUI>(true);
            if (characterInfoUI != null) characterInfoUI.gameObject.SetActive(false);

            characterBattleUI = GetComponentInChildren<CharacterBattleUI>(true);
            if (characterBattleUI != null) characterBattleUI.gameObject.SetActive(false);
        }

        public void InitInfoUI(EAttributeType type, int level)
        {
            characterInfoUI?.SetCharacterInfo(type, level);
        }

        public void InitBattleUI(Character character)
        {
            characterBattleUI?.Init(character);
        }

        private void LateUpdate()
        {
            if (target == null || mainCamera == null || 
                rectTransform == null || canvasRect == null) return;

            Vector3 viewportPos = mainCamera.WorldToViewportPoint(target.position + offset);
            if (viewportPos.z < 0)
            {
                SetVisible(false);
                return;
            }

            SetVisible(true);

            // 화면 중앙(0.5) 기준으로 보정
            float distanceFromCenter = viewportPos.x - 0.5f;

            // 보정치 적용: 중앙에서 멀어질수록 반대 방향으로 좌표를 이동시킴
            float correctedX = viewportPos.x - (distanceFromCenter * distortionFactor);

            // 최종 좌표를 캔버스 크기에 맞게 변환
            Vector2 canvasSize = canvasRect.rect.size;
            Vector2 finalPos = new Vector2(
                (correctedX * canvasSize.x) - (canvasSize.x * 0.5f), 
                (viewportPos.y * canvasSize.y) - (canvasSize.y * 0.5f));

            rectTransform.anchoredPosition = finalPos;
        }

        public void SetTarget(Canvas canvas, Transform newTarget, Vector3 uiOffset)
        {
            if (canvas != null)
            {
                canvasRect = canvas.GetComponent<RectTransform>();
                if (transform.parent != canvas.transform)
                    transform.SetParent(canvas.transform, false);
            }

            target = newTarget;
            offset = uiOffset;
            gameObject.SetActive(true);
        }

        public void ReleaseTarget()
        {
            target = null;
            offset = Vector3.zero;
            gameObject.SetActive(false);
        }

        public void ActiveInfoUI()
        {
            if (characterInfoUI != null) characterInfoUI.gameObject.SetActive(true);
            if (characterBattleUI != null) characterBattleUI.gameObject.SetActive(false);
        }

        public void ActiveBattleUI()
        {
            if (characterInfoUI != null) characterInfoUI.gameObject.SetActive(false);
            if (characterBattleUI != null) characterBattleUI.gameObject.SetActive(true);
        }
        private void SetVisible(bool visible)
        {
            if (canvasGroup != null)
                canvasGroup.alpha = visible ? 1f : 0f;
            else
                gameObject.SetActive(visible);
        }
    }
}