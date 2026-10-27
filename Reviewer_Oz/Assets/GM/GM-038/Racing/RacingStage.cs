using TMPro;
using UnityEngine;

namespace GM_038
{
    public class RacingStage : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private RacingPlayer player;
        [SerializeField] private RacingEnemy enemyPrefab;

        [Header("적 생성")]
        [SerializeField] private float spawnInterval = 1.2f;
        [SerializeField] private float spawnY = 8f;
        [SerializeField] private float[] lanePositions = { -2.2f, 0f, 2.2f };

        [Header("배경과 점수")]
        [SerializeField] private float maxScrollSpeed = 10f;
        [SerializeField] private int survivalScorePerSecond = 10;

        [Header("UI")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text speedText;
        [SerializeField] private TMP_Text skillText;
        [SerializeField] private RectTransform speedNeedle;
        [SerializeField] private GameObject gameOverRoot;
        [SerializeField] private float zeroAngle = 135f;
        [SerializeField] private float maxAngle = -135f;

        float survivalTime = 0f;
        float spawnTimer = 0f;
        int enemyScore = 0;

        public bool IsGameOver { get; private set; }

        public float ScrollSpeed =>
            IsGameOver ? 0f : player.CurrentSpeed / player.MaxSpeed * maxScrollSpeed;

        public float PassLine => player.transform.position.y - 1.8f;

        void Start()
        {
            spawnInterval = Mathf.Max(0.1f, spawnInterval);
            gameOverRoot.SetActive(false);
            RefreshUI();
        }

        void Update()
        {
            if (IsGameOver) return;

            survivalTime += Time.deltaTime;
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                spawnTimer -= spawnInterval;
                SpawnEnemy();
            }
        }

        void LateUpdate()
        {
            RefreshUI();
        }

        void SpawnEnemy()
        {
            if (enemyPrefab == null || lanePositions.Length == 0) return;

            int index = Random.Range(0, lanePositions.Length);
            Vector3 position = new Vector3(lanePositions[index], spawnY, 0f);

            RacingEnemy enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            enemy.Initialize(this);
        }

        public void AddScore(int amount)
        {
            if (IsGameOver) return;

            enemyScore += amount;
        }

        public void GameOver()
        {
            if (IsGameOver) return;

            IsGameOver = true;
            player.Stop();
            gameOverRoot.SetActive(true);

            Debug.Log($"Game Over! 최종 점수 : {GetScore()}");
        }

        int GetScore()
        {
            return enemyScore + Mathf.FloorToInt(survivalTime * survivalScorePerSecond);
        }

        void RefreshUI()
        {
            scoreText.text = $"Score : {GetScore()}";
            timeText.text = $"Time : {survivalTime:F1}s";
            speedText.text = $"{player.CurrentSpeed:F0} km/h";
            skillText.text = player.IsShieldActive ? "Q : Shield ON | E : Slash" : "Q : Shield | E : Slash";

            float ratio = player.CurrentSpeed / player.MaxSpeed;
            speedNeedle.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(zeroAngle, maxAngle, ratio));
        }
    }
}