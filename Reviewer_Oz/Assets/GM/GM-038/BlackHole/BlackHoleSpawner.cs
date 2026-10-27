using UnityEngine;

namespace GM_038
{
    public class BlackHoleSpawner : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private BlackHole blackHole;
        [SerializeField] private GameObject[] objectPrefabs;
        [SerializeField] private GameObject markerPrefab;

        [Header("생성")]
        [SerializeField] private float spawnInterval = 0.3f;
        [SerializeField] private float warningDuration = 0.5f;
        [SerializeField] private float minSpawnRadius = 2.5f;
        [SerializeField] private float maxSpawnRadius = 5f;

        GameObject currentMarker;
        Vector3 spawnPosition;

        float timer = 0f;
        bool waiting = false;

        void Start()
        {
            if (blackHole == null || markerPrefab == null ||
                objectPrefabs == null || objectPrefabs.Length == 0)
            {
                Debug.LogWarning("BlackHole과 오브젝트/마커 Prefab을 연결하세요.");
                enabled = false;
                return;
            }

            for (int i = 0; i < objectPrefabs.Length; i++)
            {
                if (objectPrefabs[i] != null) continue;

                Debug.LogWarning("Object Prefabs에 비어 있는 칸이 있습니다.");
                enabled = false;
                return;
            }

            spawnInterval = Mathf.Max(0.1f, spawnInterval);
            warningDuration = Mathf.Max(0.1f, warningDuration);
            minSpawnRadius = Mathf.Max(0f, minSpawnRadius);
            maxSpawnRadius = Mathf.Max(minSpawnRadius, maxSpawnRadius);
        }

        void Update()
        {
            timer += Time.deltaTime;

            if (waiting)
            {
                float size = Mathf.Lerp(1f, 0.3f, timer / warningDuration);
                currentMarker.transform.localScale = Vector3.one * size;

                if (timer >= warningDuration)
                {
                    SpawnObject();

                    Destroy(currentMarker);
                    currentMarker = null;

                    waiting = false;
                    timer = 0f;
                }

                return;
            }

            if (timer >= spawnInterval)
            {
                PrepareSpawn();
                timer = 0f;
                waiting = true;
            }
        }

        void PrepareSpawn()
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float radius = Random.Range(minSpawnRadius, maxSpawnRadius);

            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
            spawnPosition = blackHole.transform.position + offset;

            currentMarker = Instantiate(markerPrefab, spawnPosition, Quaternion.identity);
        }

        void SpawnObject()
        {
            int index = Random.Range(0, objectPrefabs.Length);
            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

            GameObject obj = Instantiate(objectPrefabs[index], spawnPosition, rotation);
            obj.transform.localScale *= Random.Range(0.6f, 1.2f);

            SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();

            if (sprite != null)
            {
                sprite.color = new Color(Random.Range(0.3f, 1f), Random.Range(0.3f, 1f), Random.Range(0.3f, 1f));
            }

            Debug.Log($"오브젝트 생성 : {spawnPosition}");
        }
    }
}