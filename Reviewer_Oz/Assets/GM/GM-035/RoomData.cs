using UnityEngine;

namespace GM_035
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "GM035/RoomData")]
    public class RoomData : ScriptableObject
    {
        [Header("Spawn")]
        public GameObject enemyPrefab;
        public int spawnCount = 1;

        [Header("Monster")]
        public int maxHP = 30;
        public int attackDamage = 5;
        public float moveSpeed = 2f;
        public float attackInterval = 2f;
        public float enemyScale = 1f;

        [Header("Range")]
        public float detectionRange = 8f;
        public float attackRange = 2f;

        [Header("NavMeshAgent")]
        public float agentRadius = 0.5f;
        public float agentHeight = 2f;
        public float agentBaseOffset = 0f;

        [Header("Stage")]
        public int stageNumber = 1;
        public int requiredGold = 10;
    }

}

