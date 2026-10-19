using UnityEngine;
using UnityEngine.InputSystem;

namespace GM_028
{
    public class PlayerShoot : MonoBehaviour
    {
        [Header("발사")]
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private float attackCooldown = 0.5f;
        [SerializeField] private float spawnDistance = 0.8f;

        [Header("에너지 셀")]
        [SerializeField] private int maxCells = 5;
        [SerializeField] private float cooling = 20f;
        [SerializeField] private float[] cellTemperatures = new float[5];
        [SerializeField] private bool isOverload = false;

        private float attackTimer = 0f;

        void Awake()
        {
            maxCells = Mathf.Max(1, maxCells);
            cellTemperatures = new float[maxCells];
            attackTimer = attackCooldown;
            isOverload = false;
        }

        void Update()
        {
            attackTimer += Time.deltaTime;

            float currentCooling = isOverload ? cooling * 0.5f : cooling;
            bool allCooled = true;

            for (int i = 0; i < cellTemperatures.Length; i++)
            {
                float previousTemperature = cellTemperatures[i];

                cellTemperatures[i] = Mathf.Clamp(cellTemperatures[i] - currentCooling * Time.deltaTime, 0f, 100f);

                if (previousTemperature > 0f && cellTemperatures[i] == 0f)
                {
                    Debug.Log($"셀 {i} 냉각 완료! 다시 사용할 수 있습니다.");
                }

                if (cellTemperatures[i] > 0f) allCooled = false;
            }

            if (isOverload && allCooled)
            {
                isOverload = false;
                Debug.Log("전체 셀 냉각 완료! 과부하가 해제됩니다.");
            }
        }

        public void OnShoot(InputValue value)
        {
            if (!value.isPressed) return;

            TryAttack();
        }

        void TryAttack()
        {
            if (bulletPrefab == null)
            {
                Debug.LogWarning("Bullet Prefab을 연결하세요.");
                return;
            }

            if (attackTimer < attackCooldown)
            {
                Debug.Log($"연속 발사 제한! 남은 시간 : {attackCooldown - attackTimer:F2}초");
                return;
            }

            int cellIndex = -1;

            for (int i = 0; i < cellTemperatures.Length; i++)
            {
                if (cellTemperatures[i] > 0f) continue;

                cellIndex = i;
                break;
            }

            if (cellIndex == -1)
            {
                Debug.LogWarning("사용 가능한 셀이 없습니다. 냉각을 기다리세요.");
                return;
            }

            cellTemperatures[cellIndex] = 100f;
            attackTimer = 0f;

            SpawnBullet();

            Debug.Log($"사용한 셀 : {cellIndex} | 셀 온도 : {string.Join(", ", cellTemperatures)}");

            CheckOverload();
        }

        void SpawnBullet()
        {
            Vector3 direction = transform.forward;
            Vector3 spawnPosition = transform.position + direction * spawnDistance;

            Bullet bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
            bullet.gameObject.SetActive(true);

            Debug.Log($"탄환 생성 | 위치 : {spawnPosition} | 방향 : {direction}");
        }

        void CheckOverload()
        {
            if (isOverload) return;

            for (int i = 0; i < cellTemperatures.Length; i++)
            {
                if (cellTemperatures[i] < 50f) return;
            }

            isOverload = true;
            Debug.LogWarning($"모든 셀이 50도 이상입니다! 과부하 발생! 냉각 속도 : 초당 {cooling * 0.5f}");
        }
    }
}