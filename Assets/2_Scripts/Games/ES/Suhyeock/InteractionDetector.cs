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
                nearbyInteractables.Remove(interactable);

                if (interactable == currentNearest)
                {
                    eventBroker.CloseLootDisplay();
                    currentNearest = null;
                }
            }
        }

        private void Update()
        {
            IInteractable nearest = GetNearestInteractable();

            if (nearest != currentNearest)
            {
                currentNearest = nearest;

                if (currentNearest != null && currentNearest.CanInteract())
                {
                    eventBroker.UpdateInteractionPrompt(true, currentNearest.transform);
                }
                else
                {
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

        public bool IsObjectNearby(IInteractable target)
        {
            return nearbyInteractables.Contains(target);
        }
    }
}