using UnityEngine;

namespace LUP.ES
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject MonsterPrefab1;
        [SerializeField] private GameObject MonsterPrefab2;
        [SerializeField] private Transform[] SpawnPoints;

        void Start()
        {
            SpawnAllMonsters();
        }
        void Update()
        {

        }

        void SpawnAllMonsters()
        {
            GameObject[] monsterPrefabs = new GameObject[] { MonsterPrefab1, MonsterPrefab2 };

            // 각 스폰 지점에 대해 반복합니다.
            foreach (Transform point in SpawnPoints)
            {
                // 1. 랜덤 인덱스 선택
                // 0 또는 1 중에서 랜덤 정수를 선택합니다.
                int randomIndex = Random.Range(0, monsterPrefabs.Length); // monsterPrefabs.Length는 2입니다.

                // 2. 랜덤으로 선택된 프리팹 가져오기
                GameObject monsterToSpawn = monsterPrefabs[randomIndex];

                // 3. 몬스터 생성 (스폰)
                // 선택된 몬스터를 현재 스폰 지점의 위치와 회전으로 생성합니다.
                Instantiate(
                    monsterToSpawn,
                    point.position,
                    point.rotation
                );
            }
        }
    }
}