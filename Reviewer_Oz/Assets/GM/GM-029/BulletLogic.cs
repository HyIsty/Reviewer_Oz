using UnityEngine;
using UnityEngine.InputSystem;
using GM_028;

namespace GM_029
{
    public class BulletLogic : MonoBehaviour
    {
        [Header("ÅºÈ¯ ÇÁ¸®ÆÕ")]
        [SerializeField] private Bullet bulletPrefab;

        [Header("°ø°Ý")]
        [SerializeField] private float fireInterval = 0.2f;
        [SerializeField] private float spawnDistance = 0.8f;
        [SerializeField] private int projectileCount = 5;
        [SerializeField] private float spreadAngle = 120f;

        [Header("³ª¼±Åº")]
        [SerializeField] private float currentAngle;
        [SerializeField] private float rotationSpeed = 20f;

        [Header("µð¹ö±×")]
        [SerializeField] private bool showDebugLog = true;

        private float lastAttackTime = -999f;
        private bool isSpiralShooting;

        void Awake()
        {
            projectileCount = Mathf.Max(1, projectileCount);
            fireInterval = Mathf.Max(0.01f, fireInterval);
        }

        void Update()
        {
            if (isSpiralShooting)
            {
                TrySpiralAttack();
            }
        }

        void OnDisable()
        {
            isSpiralShooting = false;
        }

        public void OnShoot(InputValue value)
        {
            if (!value.isPressed) return;

            TryAttack();
        }

        public void OnRadial(InputValue value)
        {
            if (!value.isPressed) return;

            TryRadialAttack();
        }

        public void OnSpiral(InputValue value)
        {
            isSpiralShooting = value.isPressed;

            if (!isSpiralShooting) return;

            TrySpiralAttack();
        }

        bool CanAttack()
        {
            return bulletPrefab != null;
        }

        void TryAttack()
        {
            if (!CanAttack()) return;

            float angleStep = 0f;
            float startAngle = 0f;

            if (projectileCount > 1)
            {
                angleStep = spreadAngle / (projectileCount - 1);
                startAngle = -spreadAngle * 0.5f;
            }

            for (int i = 0; i < projectileCount; i++)
            {
                float angle = startAngle + angleStep * i;
                Vector3 fireDirection = Quaternion.Euler(0f, angle, 0f) * transform.forward;

                SpawnBullet(fireDirection);
            }
        }

        void TryRadialAttack()
        {
            if (!CanAttack()) return;

            float angleStep = 360f / projectileCount;

            for (int i = 0; i < projectileCount; i++)
            {
                float angle = angleStep * i;
                Vector3 fireDirection = Quaternion.Euler(0f, angle, 0f) * transform.forward;

                SpawnBullet(fireDirection);
            }
        }

        void TrySpiralAttack()
        {
            if (!CanAttack()) return;
            if (Time.time < lastAttackTime + fireInterval) return;

            float radian = currentAngle * Mathf.Deg2Rad;
            Vector3 fireDirection = transform.forward * Mathf.Cos(radian) + transform.right * Mathf.Sin(radian);

            SpawnBullet(fireDirection);

            currentAngle = (currentAngle + rotationSpeed) % 360f;
            lastAttackTime = Time.time;
        }

        void SpawnBullet(Vector3 fireDirection)
        {
            Vector3 spawnPosition = transform.position + fireDirection * spawnDistance;
            Quaternion rotation = Quaternion.LookRotation(fireDirection);

            Bullet bullet = Instantiate(bulletPrefab, spawnPosition, rotation);
            bullet.gameObject.SetActive(true);

            if (showDebugLog)
            {
                Debug.Log($"ÅºÈ¯ »ý¼º | À§Ä¡ : {spawnPosition} | ¹æÇâ : {fireDirection}");
            }
        }

    }
}