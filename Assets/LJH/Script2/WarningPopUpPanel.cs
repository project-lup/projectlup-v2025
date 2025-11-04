using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Roguelike.Util;

namespace RK
{
    public class WarningPopUpPanel : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public UnityEngine.UI.Button OkBtn;
        public Animator PadeInOutAnim;

        public float FadeOutTime = 0.2f;

        void Start()
        {
            OkBtn.onClick.AddListener(PlayReverse);
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            PadeInOutAnim.Play("PadeIn");
            PadeInOutAnim.speed = 1.0f;
        }

        void PlayReverse()
        {
            PadeInOutAnim.Play("PadeOut");
            PadeInOutAnim.speed = 1.0f;

            StartCoroutine(RoguelikeUtil.DelayForSeconds(0.2f, DisablePanel));
        }

        void DisablePanel()
        {
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

