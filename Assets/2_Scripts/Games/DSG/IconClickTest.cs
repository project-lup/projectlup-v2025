using System.Collections;
using UnityEngine;

namespace LUP.DSG
{
    public class IconClickTest : MonoBehaviour
    {
        [Header("테스트 설정")]
        [Tooltip("선택→해제를 1회로 카운트. 총 반복 횟수")]
        [SerializeField] private int repeatCount = 30;

        [Tooltip("선택 후 해제까지 대기 시간(초)")]
        [SerializeField] private float clickInterval = 0.3f;

        [Tooltip("테스트할 아이콘 인덱스 (리스트 내 순서)")]
        [SerializeField] private int targetIconIndex = 0;

        private bool isRunning = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1) && !isRunning)
                StartCoroutine(RunClickTest());
        }

        private IEnumerator RunClickTest()
        {
            isRunning = true;

            // 씬에서 CharacterIcon 검색
            CharacterIcon[] icons = FindObjectsByType<CharacterIcon>(FindObjectsSortMode.None);

            if (icons == null || icons.Length == 0)
            {
                Debug.LogError("[ProfileTest] CharacterIcon을 찾을 수 없습니다.");
                isRunning = false;
                yield break;
            }

            if (targetIconIndex >= icons.Length)
            {
                Debug.LogError($"[ProfileTest] targetIconIndex({targetIconIndex})가 범위를 초과합니다. 아이콘 수: {icons.Length}");
                isRunning = false;
                yield break;
            }

            CharacterIcon target = icons[targetIconIndex];
            Debug.Log($"[ProfileTest] 테스트 시작 — 반복 {repeatCount}회, 대상 아이콘 index: {targetIconIndex}");

            // 시작 전 선택 상태라면 먼저 해제
            if (target.selectedButton != null && target.selectedButton.isSelected)
            {
                target.OnButtonClicked();
                yield return new WaitForSeconds(clickInterval);
            }

            for (int i = 0; i < repeatCount; i++)
            {
                // 선택 (Place)
                target.OnButtonClicked();
                yield return new WaitForSeconds(clickInterval);

                // 해제 (Release)
                target.OnButtonClicked();
                yield return new WaitForSeconds(clickInterval);
            }

            Debug.Log($"[ProfileTest] 테스트 완료 — {repeatCount}회 선택/해제 수행됨");
            isRunning = false;
        }
    }
}