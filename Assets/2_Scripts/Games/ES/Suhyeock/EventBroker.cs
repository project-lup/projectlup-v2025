using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class EventBroker : MonoBehaviour
    {
        public Action<bool> OnGameFinished;

        public Action<bool> OnInventoryVisibilityChanged;
        public Action<List<Item>> OnOpenLootDisplay;
        public Action OnCloseLootDisplay;

        public Action<float, float> OnReloadTimeUpdate;
        public Action<float, float> OnPlayerHPUpdated;

        public Action<bool, Transform> OnInteractionPromptStateChanged; // 프롬프트 켜기/끄기 및 대상 위치
        public Action<float, float> OnInteractionTimerUpdated;

        // 기수 추가한 코드
        public Action ExtractionSuccess;
        private bool isGameFinished = false;

        public void ReportGameFinish(bool isSuccess)
        {
            if (isGameFinished) return;  // 기수 추가한 코드

            isGameFinished = true;       // 기수 추가한 코드
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

        // 기수 추가한 코드
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

        public void UpdateInteractionTimer(float currentTime, float maxTime)
        {
            OnInteractionTimerUpdated?.Invoke(currentTime, maxTime);
        }
    }
}