using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class InteractionDetector : MonoBehaviour
    {
        [HideInInspector]
        public EventBroker eventBroker;
        public PlayerBlackboard blackboard;
        private SphereCollider detectionCollider;
        private List<IInteractable> nearbyInteractables = new List<IInteractable>();

        private IInteractable currentNearest = null;

        private void Start()
        {
            eventBroker = FindAnyObjectByType<EventBroker>();
            detectionCollider = gameObject.AddComponent<SphereCollider>();
            detectionCollider.radius = blackboard.InteractionRadius;
            detectionCollider.isTrigger = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out IInteractable interactable))
            {
                nearbyInteractables.Add(interactable);
                Debug.Log("Count: " + nearbyInteractables.Count);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                eventBroker.CloseLootDisplay();
                nearbyInteractables.Remove(interactable);
            }
        }

        private void Update()
        {
            IInteractable nearest = GetNearestInteractable();

            // 타겟이 바뀌었을 때만 EventBroker에 방송합니다 (최적화)
            if (nearest != currentNearest)
            {
                currentNearest = nearest;

                if (currentNearest != null && currentNearest.CanInteract())
                {
                    // 타겟이 존재하면 "UI 켜라!" 방송
                    eventBroker.UpdateInteractionPrompt(true, currentNearest.transform);
                }
                else
                {
                    // 타겟이 범위에서 벗어났거나, 상호작용 불가능해지면 "UI 꺼라!" 방송
                    eventBroker.UpdateInteractionPrompt(false);
                }
            }
        }

        public IInteractable GetNearestInteractable()
        {
            if (nearbyInteractables.Count == 0)
                return null;

            IInteractable nearest = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < nearbyInteractables.Count; i++)
            {
                GameObject obj = (nearbyInteractables[i] as MonoBehaviour)?.gameObject;
                if (obj == null) continue;

                if (nearbyInteractables[i].CanInteract())
                {
                    float distance = Vector3.Distance(transform.position, obj.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = nearbyInteractables[i];
                    }
                }
            }
            return nearest;
        }

        // 기수 추가한 코드
        public bool IsObjectNearby(IInteractable target)
        {
            return nearbyInteractables.Contains(target);
        }
    }
}