using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace DSG
{
    public class CharactersList : MonoBehaviour
    {
        [SerializeField]
        private DataCenter dataCenter;

        [SerializeField]
        private GameObject iconPrefab;
        [SerializeField]
        private Transform contentParent;

        private void Start()
        {
            PopulateScrollView();
        }

        private void PopulateScrollView()
        {
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }

            List<OwnedCharacterInfo> characterList = dataCenter.GetOwnedCharacterList();

            if (characterList == null) return;

            foreach (OwnedCharacterInfo character in characterList)
            {
                var itemUI = Instantiate(iconPrefab, contentParent.transform);
                var slot = itemUI.GetComponent<CharacterIcon>(); // 프리팹에 붙은 UI 스크립트

                var characterData = dataCenter.FindCharacterData(character.characterID);
                //var statusData = dataCenter.GetCharacterStatus(character.characterID, character.characterLevel);
                var modelData = dataCenter.FindCharacterModel(character.characterModelID);

                slot.SetData(characterData, modelData, character.characterLevel);
            }
        }
    }
}