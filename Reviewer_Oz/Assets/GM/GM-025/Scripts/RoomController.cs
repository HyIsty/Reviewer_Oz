using UnityEngine;

namespace GM_025
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class RoomController : MonoBehaviour
    {
        [SerializeField] private RoomData roomData;
        [SerializeField] private PlayerController player;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject exitDoor;
        [SerializeField] private bool isLastRoom;

        bool isStarted = false;
        int remainingEnemy;

        void OnTriggerEnter(Collider other)
        {
            if (isStarted || player == null) return;
            if (other.GetComponentInParent<PlayerController>() != player) return;

            StartRoom();
        }

        void StartRoom()
        {
            if (roomData == null || roomData.enemyPrefab == null)
            {
                Debug.LogWarning("RoomData와 Enemy Prefab을 연결하세요.");
                return;
            }

            if (roomData.enemyPrefab.GetComponent<EnemyAI>() == null)
            {
                Debug.LogWarning("Enemy 프리팹의 루트에 EnemyAI가 필요합니다.");
                return;
            }

            if (roomData.spawnCount <= 0 || spawnPoints.Length < roomData.spawnCount)
            {
                Debug.LogWarning("적 생성 수와 스폰 위치 개수를 확인하세요.");
                return;
            }

            for (int i = 0; i < roomData.spawnCount; i++)
            {
                if (spawnPoints[i] == null)
                {
                    Debug.LogWarning("비어 있는 스폰 위치를 연결하세요.");
                    return;
                }
            }

            isStarted = true;
            remainingEnemy = roomData.spawnCount;

            for (int i = 0; i < roomData.spawnCount; i++)
            {
                GameObject enemyObject = Instantiate(roomData.enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
                EnemyAI enemy = enemyObject.GetComponent<EnemyAI>();

                enemy.Initialize(roomData, player);
                enemy.onDeath += EnemyDead;
            }

            Debug.Log($"{roomData.name} 시작! 적 {remainingEnemy}마리 등장!");
        }

        void EnemyDead()
        {
            remainingEnemy--;

            Debug.Log($"남은 적 : {remainingEnemy}");

            if (remainingEnemy > 0) return;

            Debug.Log($"{roomData.name} 클리어!");

            if (exitDoor != null) exitDoor.SetActive(false);

            if (isLastRoom)
            {
                Debug.Log("모든 방을 클리어했습니다!");
            }
        }
    }
}