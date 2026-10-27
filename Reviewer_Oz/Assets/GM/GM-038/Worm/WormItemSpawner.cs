using System.Collections.Generic;
using UnityEngine;

namespace GM_038
{
    public class WormItemSpawner : MonoBehaviour
    {
        [SerializeField] private WormController worm;
        [SerializeField] private WormItem itemPrefab;
        [SerializeField] private float startDelay = 5f;
        [SerializeField] private float spawnInterval = 2f;

        List<WormItem> items = new List<WormItem>();

        float timer = 0f;
        bool spawning = false;

        void Start()
        {
            if (worm == null || itemPrefab == null)
            {
                Debug.LogWarning("Worm과 Item Prefab을 연결하세요.");
                enabled = false;
                return;
            }

            spawnInterval = Mathf.Max(0.1f, spawnInterval);
        }

        void Update()
        {
            if (!worm.IsReady) return;

            timer += Time.deltaTime;

            if (!spawning && timer >= startDelay)
            {
                spawning = true;
                timer = 0f;
                SpawnItem();
            }
            else if (spawning && timer >= spawnInterval)
            {
                timer -= spawnInterval;
                SpawnItem();
            }

            CheckPickup();
        }

        void SpawnItem()
        {
            Vector3 position = new Vector3(
                Random.Range(worm.MinBounds.x + 0.7f, worm.MaxBounds.x - 0.7f),
                Random.Range(worm.MinBounds.y + 0.7f, worm.MaxBounds.y - 0.7f), 0f);

            int number = Random.Range(1, worm.SegmentCount + 1);

            WormItem item = Instantiate(itemPrefab, position, Quaternion.identity);
            item.Initialize(number);

            items.Add(item);
        }

        void CheckPickup()
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                WormItem item = items[i];

                if (item == null)
                {
                    items.RemoveAt(i);
                    continue;
                }

                float distance = Vector2.Distance(worm.HeadPosition, item.transform.position);

                if (distance > worm.HeadRadius + item.Radius) continue;

                worm.GrowSegment(item.Number);
                item.Collect();

                items.RemoveAt(i);
            }
        }
    }
}