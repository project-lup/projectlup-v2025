using LUP.ES;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class MovingPlatformController : MonoBehaviour, IInteractable
    {
        [Header("설정")]
        public float interactionDuration = 3f;
        public MovingPlatform platform;

        [Header("상호작용")]
        public bool isInteracting { get; private set; } = false;
        public float currentInteractionTime { get; private set; } = 0f;

        private EventBroker eventBroker;

        public bool InterruptsOnMove => true;
        public bool CanInteract() => !isInteracting;

        void Start()
        {
            eventBroker = FindAnyObjectByType<EventBroker>();
            //HideInteractionPrompt();
        }

        public void Interact()
        {
            // 상호작용 시작, 타이머 UI 표시
            isInteracting = true;
            currentInteractionTime = interactionDuration;
            Debug.Log("엘리베이터 상호작용 시작!");
        }

        public bool TryStartInteraction(float deltaTime)
        {
            if (!isInteracting)
            {
                Interact();
                return false;
            }

            currentInteractionTime -= deltaTime;
            eventBroker.UpdateInteractionTimer(interactionDuration - currentInteractionTime, interactionDuration);

            if (currentInteractionTime <= 0f)
            {
                isInteracting = false;

                // 타이머 완료 시 실제 엘리베이터 이동
                if (platform != null)
                    platform.StartMove();

                return true;
            }

            return false;
        }

        public void ResetInteraction()
        {
            isInteracting = false;
            currentInteractionTime = 0f;
        }

    }
}