using System.Collections.Generic;
using UnityEngine;
using GM_030;

namespace GM_033
{
    [System.Serializable]
    public struct DropItem
    {
        public string itemId;
        [Min(1)] public int amount;
        [Range(0f, 100f)] public float dropPercentage;
    }

    [System.Serializable]
    public struct MonsterLootData
    {
        public string monsterId;
        public DropItem[] dropItems;
    }

    public class LootManager : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private ItemCatalogManager itemCatalog;
        [SerializeField] private WorldItem worldItemPrefab;

        [Header("몬스터별 드롭 테이블")]
        [SerializeField] private MonsterLootData[] monsterTables;

        [Header("배치 설정")]
        [SerializeField] private float spawnRadius = 1f;
        [SerializeField] private float radiusStep = 1f;
        [SerializeField] private float itemCheckRadius = 0.3f;
        [SerializeField] private int maxAttempts = 100;
        [SerializeField] private LayerMask obstacleLayer;

        private readonly Dictionary<string, List<DropItem>> lootTableDB = new Dictionary<string, List<DropItem>>();

        private readonly Vector3[] baseDirections =
        {
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 0f, 1f).normalized,
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 0f, -1f).normalized,
            new Vector3(0f, 0f, -1f),
            new Vector3(-1f, 0f, -1f).normalized,
            new Vector3(-1f, 0f, 0f),
            new Vector3(-1f, 0f, 1f).normalized
        };

        void Awake()
        {
            if (itemCatalog == null) itemCatalog = GetComponent<ItemCatalogManager>();

            spawnRadius = Mathf.Max(0.1f, spawnRadius);
            radiusStep = Mathf.Max(0.1f, radiusStep);
            itemCheckRadius = Mathf.Max(0.01f, itemCheckRadius);
            maxAttempts = Mathf.Max(1, maxAttempts);

            lootTableDB.Clear();

            if (monsterTables == null || monsterTables.Length == 0)
            {
                Debug.LogWarning("[LootManager] 드롭 테이블이 비어 있습니다.");
                return;
            }

            foreach (MonsterLootData data in monsterTables)
            {
                if (string.IsNullOrEmpty(data.monsterId) || data.dropItems == null || data.dropItems.Length == 0)
                {
                    Debug.LogWarning("[LootManager] 몬스터 ID 또는 드롭 목록을 확인하세요.");
                    continue;
                }

                if (lootTableDB.ContainsKey(data.monsterId))
                {
                    Debug.LogWarning($"중복된 몬스터 ID입니다. : {data.monsterId}");
                    continue;
                }

                lootTableDB.Add(data.monsterId, new List<DropItem>(data.dropItems));
            }
        }

        public void OnMonsterDeath(string monsterId, Vector3 deathPosition)
        {
            if (itemCatalog == null || worldItemPrefab == null)
            {
                Debug.LogWarning("[LootManager] ItemCatalog와 WorldItem Prefab을 연결하세요.");
                return;
            }

            if (string.IsNullOrEmpty(monsterId))
            {
                Debug.LogWarning("[LootManager] 몬스터 ID가 비어 있습니다.");
                return;
            }

            if (!lootTableDB.TryGetValue(monsterId, out List<DropItem> table))
            {
                Debug.LogWarning($"등록된 드롭 테이블이 없습니다. : {monsterId}");
                return;
            }

            List<DropItem> itemsToDrop = new List<DropItem>();

            foreach (DropItem item in table)
            {
                if (!itemCatalog.IsRegistered(item.itemId))
                {
                    Debug.LogWarning($"카탈로그에 등록되지 않은 아이템입니다. : {item.itemId}");
                    continue;
                }

                if (item.amount <= 0)
                {
                    Debug.LogWarning($"드롭 수량을 확인하세요. : {item.itemId} / {item.amount}");
                    continue;
                }

                float chance = Mathf.Clamp(item.dropPercentage, 0f, 100f);

                if (chance <= 0f) continue;

                if (chance >= 100f || Random.Range(0f, 100f) < chance)
                {
                    itemsToDrop.Add(item);
                }
            }

            if (itemsToDrop.Count == 0)
            {
                Debug.Log($"드롭된 아이템이 없습니다. : {monsterId}");
                return;
            }

            SpawnItemsDynamically(itemsToDrop, deathPosition);
        }

        void SpawnItemsDynamically(List<DropItem> items, Vector3 centerPosition)
        {
            int spawnedCount = 0;

            for (int attempt = 0; attempt < maxAttempts && spawnedCount < items.Count; attempt++)
            {
                int directionIndex = attempt % baseDirections.Length;
                int ringIndex = attempt / baseDirections.Length;
                float radius = spawnRadius + ringIndex * radiusStep;

                Vector3 spawnPosition = centerPosition + baseDirections[directionIndex] * radius;

                if (Physics.CheckSphere(spawnPosition, itemCheckRadius, obstacleLayer, QueryTriggerInteraction.Collide))
                {
                    continue;
                }

                SpawnItem(items[spawnedCount], spawnPosition);
                spawnedCount++;
            }

            Debug.Log($"드롭 생성 완료 : {spawnedCount} / {items.Count}개");

            if (spawnedCount < items.Count)
            {
                Debug.LogWarning($"빈 공간을 찾지 못해 {items.Count - spawnedCount}개를 생성하지 못했습니다.");
            }
        }

        void SpawnItem(DropItem dropData, Vector3 spawnPosition)
        {
            if (!itemCatalog.TryGetItem(dropData.itemId, out ItemData itemData)) return;

            WorldItem item = Instantiate(worldItemPrefab, spawnPosition, Quaternion.identity);

            item.itemId = itemData.itemId;
            item.itemDisplayName = string.IsNullOrEmpty(itemData.displayName) ? itemData.itemId : itemData.displayName;
            item.amount = dropData.amount;
            item.itemType = itemData.itemType;
            item.keyType = itemData.keyType;

            item.gameObject.SetActive(true);
        }
    }
}
