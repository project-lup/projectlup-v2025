using UnityEngine;
using UnityEngine.AI;

namespace LUP.ES
{
    public class LootSpawner : MonoBehaviour
    {
        public GameObject itemBoxPrefab;
        public LayerMask obstacleLayer;

        public float searchRadius = 2.0f; 
        public int maxAttempts = 10;      
        public float offsetY = 0.5f;      
        public float checkRadius = 0.5f;  

        public void SpawnLoot()
        {
            if (itemBoxPrefab == null)
                return;
            
            Vector3 bestPosition = Vector3.zero;
            bool validPositionFound = false;

            for (int i = 0; i < maxAttempts; i++)
            {
                Vector3 randomOffset = (i == 0) ? Vector3.zero : Random.insideUnitSphere * searchRadius;
                Vector3 sourcePosition = transform.position + randomOffset;

                if (NavMesh.SamplePosition(sourcePosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    if (!Physics.CheckSphere(hit.position + Vector3.up * offsetY, checkRadius, obstacleLayer))
                    {
                        bestPosition = hit.position;
                        validPositionFound = true;
                        break;
                    }
                }
            }

            // 유효한 위치를 찾았다면 생성
            if (validPositionFound)
            {
                // 바닥 위치 + 높이 보정(yOffset)
                Instantiate(itemBoxPrefab, bestPosition + Vector3.up * offsetY, Quaternion.identity);
            }
            else
            {
                Instantiate(itemBoxPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}

