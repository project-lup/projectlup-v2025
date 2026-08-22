using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class PlayerCenter : MonoBehaviour
    {
        [System.Serializable]
        public class PlayerData
        {
            public int id;
            public GameObject prefab;
        }

        public List<PlayerData> playerList;
        private ExtractionShooterStage extractionShooterStage;

        private Dictionary<int, GameObject> playerDictionary;
        private GameObject currentCharacterInstance;

        public PlayerSpawner playerSpawner;

        private void Awake()
        {
            // 게임 시작 시 리스트를 딕셔너리로 변환 (최적화)
            playerDictionary = new Dictionary<int, GameObject>();

            foreach (var data in playerList)
            {
                if (!playerDictionary.ContainsKey(data.id))
                {
                    playerDictionary.Add(data.id, data.prefab);
                }
            }
            SpawnPlayer();
        }


        public void SpawnPlayer()
        {
            Debug.Log("SpawnPlayer");
            extractionShooterStage = StageManager.Instance.GetCurrentStage() as ExtractionShooterStage;

            int playerID = extractionShooterStage.RuntimeData.PlayerID;
            int weaponID = extractionShooterStage.RuntimeData.WeaponID;
            if (playerDictionary.TryGetValue(playerID, out GameObject prefabToSpawn))
            {
                Transform spawnPoint = playerSpawner.GetPlayerSpawnPoint();
                currentCharacterInstance = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
                PlayerBlackboard playerBlackboard =  currentCharacterInstance.GetComponent<PlayerBlackboard>();
                if (playerBlackboard != null)
                {
                    playerBlackboard.CurrentWeaponID = weaponID;
                }
            }
        }
    }

}

