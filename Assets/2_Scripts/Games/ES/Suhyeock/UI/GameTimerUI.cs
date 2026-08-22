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

        public float RemainingTime => RenamingTimer;

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

            if (currentTotalSeconds != lastDisplayedSeconds)
            {
                lastDisplayedSeconds = currentTotalSeconds;

                float Minutes = Mathf.FloorToInt(RenamingTimer / 60.0f);
                float Seconds = Mathf.FloorToInt(RenamingTimer % 60.0f);

                TimerText.SetText("{0:00}:{1:00}", Minutes, Seconds);
            }
        }
    }
}
