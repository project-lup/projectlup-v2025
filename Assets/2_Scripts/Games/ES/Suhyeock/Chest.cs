using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [Header("Open FX")]
        public GameObject openFxPrefab;
        public Transform fxAnchor;

        private GameObject fxInstance;
        private ParticleSystem[] fxSystems;

        private EventBroker eventBroker;
        public ItemCenter itemCenter;

        private float currentTime = 0.0f;
        [SerializeField]
        private float interactionDuration = 5.0f;
        private bool isInteracted = false;
        private bool isInteracting = false;
        [Header("Animation")]
        [SerializeField] private Animator chestAnimator;
        public bool InterruptsOnMove => true;

        private List<Item> dropItems = new List<Item>();
        public bool CanInteract() => !isInteracting;

        private void Start()
        {
            eventBroker = FindAnyObjectByType<EventBroker>();
            itemCenter = FindAnyObjectByType<ItemCenter>();

            Vector3 originalScale = transform.localScale;
            transform.localScale = Vector3.zero;
            transform.DOScale(originalScale, 0.5f).SetEase(Ease.OutBack);
            SoundManager.Instance.PlaySFX("ChestGenerate", gameObject);
        }
        public void Interact()
        {
            Debug.Log("Interacted");
            ResetInteraction();

            chestAnimator.SetBool("IsOpened", true);

            if (isInteracted == false)
            {
                dropItems = itemCenter.GenerateLoot();
            }

            StopOpenFX();

            eventBroker.OpenLootDisplay(dropItems);
            eventBroker.HandleIventoryVisibility(true);
            isInteracted = true;

            SoundManager.Instance.PlaySFX("ChestOpen", gameObject);
        }

        public bool TryStartInteraction(float deltaTime)
        {
            if(!isInteracting)
            {
                if (isInteracted)
                {
                    Interact();
                    return true;
                }

                PlayOpenFX();

                isInteracting = true;
                currentTime = interactionDuration;
                return false;
            }

            currentTime -= deltaTime;
            eventBroker.UpdateInteractionTimer(
                interactionDuration - currentTime, 
                interactionDuration);

            if (currentTime < 0.0f)
            {
                Interact();
            
                return true;
            }
            return false;
        }

        public void ResetInteraction()
        {
            isInteracting = false;

            StopOpenFX();

            currentTime = 0.0f;
            eventBroker.UpdateInteractionTimer(0f, interactionDuration);
        }

        void PlayOpenFX()
        {
            if (fxInstance != null) return;

            fxInstance = Instantiate(openFxPrefab, fxAnchor.position, fxAnchor.rotation, fxAnchor);
            fxSystems = fxInstance.GetComponentsInChildren<ParticleSystem>();

            foreach (var ps in fxSystems)
            {
                var main = ps.main;
                main.loop = true;
                ps.Play();
            }
        }

        void StopOpenFX()
        {
            if (fxSystems == null) return;

            foreach (var ps in fxSystems)
            {
                if (ps == null) continue;
                var main = ps.main;
                main.loop = false;
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            Destroy(fxInstance, 2.0f);
            fxInstance = null;
            fxSystems = null;
        }
    }
}
