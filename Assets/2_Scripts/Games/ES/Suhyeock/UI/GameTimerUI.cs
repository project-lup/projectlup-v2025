using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.ES
{
    public class GameTimerUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI TimerText;
        [SerializeField]
        private int TimeLimitMinutes = 10;

        private float RenamingTimer = 0.0f;
        private bool TimeIsRunning = true;

        public float RemainingTime => RenamingTimer; // 기수 추가한 코드

        private int lastDisplayedSeconds = -1;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            RenamingTimer = TimeLimitMinutes * 60.0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (TimeIsRunning)
            {
                if (RenamingTimer > 0.0f)
                {
                    RenamingTimer -= Time.deltaTime;
                }
                else
                {
                    RenamingTimer = 0.0f;
                    TimeIsRunning = false;
                }
                DisplayTime();
            }    
        }

        void DisplayTime()
        {
            //float Minutes = Mathf.FloorToInt(RenamingTimer / 60.0f);
            //float Seconds = Mathf.FloorToInt(RenamingTimer % 60.0f);

            //TimerText.text = string.Format("{0:00}:{1:00}", Minutes, Seconds);

            int currentTotalSeconds = Mathf.FloorToInt(RenamingTimer);

            // 이전 초와 다를 때만 텍스트를 갱신 (즉, 1초에 1번만 실행되어 부하 극적 감소!)
            if (currentTotalSeconds != lastDisplayedSeconds)
            {
                lastDisplayedSeconds = currentTotalSeconds;

                float Minutes = Mathf.FloorToInt(RenamingTimer / 60.0f);
                float Seconds = Mathf.FloorToInt(RenamingTimer % 60.0f);

                // string.Format을 쓰지 않고, TextMeshPro의 최적화된 SetText 함수 사용
                TimerText.SetText("{0:00}:{1:00}", Minutes, Seconds);
            }
        }
    }
}
