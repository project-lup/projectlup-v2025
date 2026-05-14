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

        //[Header("왜곡 보정")]
        //[Range(0f, 2f)]
        //public float distortionStrength = 1f;

        //// FOV가 자주 바뀌지 않는다는 가정으로 캐싱
        //private float _cachedFov = -1f;
        //private float _cachedDistortionFactor;


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

        //private float CalculateDistortionFactor()
        //{
        //    if (mainCamera == null) return 0f;

        //    float fov = mainCamera.fieldOfView;
        //    if (!Mathf.Approximately(fov, _cachedFov))
        //    {
        //        _cachedFov = fov;
        //        // 원근 카메라의 가장자리 1차 보정. 0.1은 기존 0.0~0.2 범위와 맞추기 위한 스케일
        //        _cachedDistortionFactor = Mathf.Tan(fov * Mathf.Deg2Rad * 0.5f) * distortionStrength * 0.1f;
        //    }
        //    return _cachedDistortionFactor;
        //}

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

            //float factor = CalculateDistortionFactor();

            //// 화면 중앙(0.5) 기준으로 보정
            float distanceFromCenter = viewportPos.x - 0.5f;
            //float correctedX = viewportPos.x - (distanceFromCenter * factor);
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