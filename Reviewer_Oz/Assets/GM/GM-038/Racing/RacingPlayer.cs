using UnityEngine;
using UnityEngine.InputSystem;

namespace GM_038
{
    public class RacingPlayer : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private RacingStage stage;

        [Header("운전")]
        [SerializeField] private float steerSpeed = 5f;
        [SerializeField] private float limitX = 3.2f;
        [SerializeField] private float maxSpeed = 200f;
        [SerializeField] private float acceleration = 45f;
        [SerializeField] private float deceleration = 30f;

        [Header("보호막")]
        [SerializeField] private GameObject shieldVisual;
        [SerializeField] private float shieldDuration = 3f;

        [Header("가르기")]
        [SerializeField] private Transform slashPoint;
        [SerializeField] private GameObject slashVisual;
        [SerializeField] private Vector2 slashSize = new Vector2(6f, 2f);
        [SerializeField] private float slashDuration = 0.2f;

        [Header("현재 속력")]
        [SerializeField] private float currentSpeed = 0f;

        Rigidbody2D rb;

        float moveInput = 0f;
        bool accelerating = false;
        float shieldTimer = 0f;
        float slashTimer = 0f;

        public float CurrentSpeed => currentSpeed;
        public float MaxSpeed => maxSpeed;
        public bool IsShieldActive => shieldTimer > 0f;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            maxSpeed = Mathf.Max(1f, maxSpeed);
            currentSpeed = 0f;
        }

        void Start()
        {
            shieldVisual.SetActive(false);
            slashVisual.SetActive(false);
        }

        void Update()
        {
            if (stage.IsGameOver) return;

            float targetSpeed = accelerating ? maxSpeed : 0f;
            float changeSpeed = accelerating ? acceleration : deceleration;

            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, changeSpeed * Time.deltaTime);

            shieldTimer = Mathf.Max(0f, shieldTimer - Time.deltaTime);
            slashTimer = Mathf.Max(0f, slashTimer - Time.deltaTime);

            shieldVisual.SetActive(shieldTimer > 0f);
            slashVisual.SetActive(slashTimer > 0f);
        }

        void FixedUpdate()
        {
            if (stage.IsGameOver)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            float direction = currentSpeed > 0f ? moveInput : 0f;
            float nextX = Mathf.Clamp(rb.position.x + direction * steerSpeed * Time.fixedDeltaTime, -limitX, limitX);

            rb.linearVelocity = new Vector2((nextX - rb.position.x) / Time.fixedDeltaTime, 0f);
        }

        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>().x;
        }

        public void OnAccelerate(InputValue value)
        {
            accelerating = value.isPressed;
        }

        public void OnShield(InputValue value)
        {
            if (!value.isPressed || stage.IsGameOver || IsShieldActive) return;

            shieldTimer = shieldDuration;
            shieldVisual.SetActive(true);

            Debug.Log("보호막 사용!");
        }

        public void OnSlash(InputValue value)
        {
            if (!value.isPressed || stage.IsGameOver) return;

            slashTimer = slashDuration;
            slashVisual.SetActive(true);

            Collider2D[] hits = Physics2D.OverlapBoxAll(slashPoint.position, slashSize, 0f);

            for (int i = 0; i < hits.Length; i++)
            {
                RacingEnemy enemy = hits[i].GetComponentInParent<RacingEnemy>();

                if (enemy != null) enemy.DestroyEnemy();
            }

            Debug.Log("전방 가르기 사용!");
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (stage.IsGameOver) return;

            RacingEnemy enemy = other.GetComponentInParent<RacingEnemy>();

            if (enemy == null || enemy.IsResolved) return;

            if (IsShieldActive)
            {
                enemy.DestroyEnemy();
            }
            else
            {
                stage.GameOver();
            }
        }

        public void Stop()
        {
            currentSpeed = 0f;
            shieldTimer = 0f;
            slashTimer = 0f;
            rb.linearVelocity = Vector2.zero;

            shieldVisual.SetActive(false);
            slashVisual.SetActive(false);
        }

        void OnDrawGizmosSelected()
        {
            if (slashPoint == null) return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(slashPoint.position, new Vector3(slashSize.x, slashSize.y, 0f));
        }
    }
}