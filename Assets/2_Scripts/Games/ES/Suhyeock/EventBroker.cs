using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class EventBroker : MonoBehaviour
    {
        public event Action<bool> OnGameFinished;

        public event Action<bool> OnInventoryVisibilityChanged;
        public event Action<List<Item>> OnOpenLootDisplay;
        public event Action OnCloseLootDisplay;

        public event Action<float, float> OnReloadTimeUpdate;
        public event Action<float, float> OnPlayerHPUpdated;

        public event Action<bool, Transform> OnInteractionPromptStateChanged; // 프롬프트 켜기/끄기 및 대상 위치

        public event Action ExtractionSuccess;
        private bool isGameFinished = false;

        public void ReportGameFinish(bool isSuccess)
        {
            if (isGameFinished) return;

            isGameFinished = true;
            OnGameFinished?.Invoke(isSuccess);
        }

        public void HandleIventoryVisibility(bool isVisible)
        {
            OnInventoryVisibilityChanged?.Invoke(isVisible);
        }

        public void OpenLootDisplay(List<Item> items)
        {
            OnOpenLootDisplay?.Invoke(items);
        }
        public void CloseLootDisplay()
        {
            OnCloseLootDisplay?.Invoke();
        }

        public void ReloadTimeUpdate(float time, float reloadTime)
        {
            OnReloadTimeUpdate?.Invoke(time, reloadTime);
        }

        public void OnExtractionSuccess()
        {
            Debug.Log("EventBroker: Extraction Success Triggered!");
            ExtractionSuccess?.Invoke();
        }

        public void UpdatePlayerHP(float currentHP, float maxHP)
        {
            OnPlayerHPUpdated?.Invoke(currentHP, maxHP);
        }

        public void UpdateInteractionPrompt(bool isVisible, Transform targetTransform = null)
        {
            OnInteractionPromptStateChanged?.Invoke(isVisible, targetTransform);
        }

        public event Action<float, float> OnInteractionTimerUpdated;
        public void UpdateInteractionTimer(float currentTime, float maxTime)
        {
            OnInteractionTimerUpdated?.Invoke(currentTime, maxTime);
        }
    }
}