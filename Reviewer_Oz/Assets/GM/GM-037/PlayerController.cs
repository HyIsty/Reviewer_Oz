using UnityEngine;
using UnityEngine.InputSystem;

namespace GM_037
{
    public class PlayerController : MonoBehaviour
    {
        [Header("이동")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float jumpForce = 8f;

        [Header("바닥 판정")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance = 0.1f;

        [Header("리스폰")]
        [SerializeField] private Transform checkpoint;
        [SerializeField] private float fallHeight = -5.5f;

        Rigidbody2D rb;
        BoxCollider2D bodyCollider;

        Vector2 moveInput;
        Vector2 startPosition;
        MovingObject currentPlatform;

        bool jumpRequested = false;
        bool jumping = false;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            bodyCollider = GetComponent<BoxCollider2D>();
            startPosition = rb.position;
        }

        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        public void OnJump(InputValue value)
        {
            if (value.isPressed) jumpRequested = true;
        }

        void FixedUpdate()
        {
            if (rb.position.y < fallHeight)
            {
                Respawn();
                return;
            }

            if (jumping && rb.linearVelocity.y <= 0f) jumping = false;

            Bounds bounds = bodyCollider.bounds;
            Vector2 origin = new Vector2(bounds.center.x, bounds.min.y + 0.05f);
            Vector2 size = new Vector2(bounds.size.x * 0.8f, 0.05f);

            RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, groundCheckDistance, groundLayer);
            bool grounded = hit.collider != null && hit.normal.y > 0.5f && !jumping;

            MovingObject platform = null;

            if (grounded) platform = hit.collider.GetComponentInParent<MovingObject>();

            SetPlatform(platform);

            Vector2 platformVelocity = currentPlatform != null ? currentPlatform.PrepareMovement() : Vector2.zero;
            Vector2 velocity = rb.linearVelocity;

            velocity.x = moveInput.x * moveSpeed + platformVelocity.x;

            if (grounded) velocity.y = platformVelocity.y;

            if (jumpRequested && grounded)
            {
                velocity.y = jumpForce + platformVelocity.y;
                jumping = true;
                SetPlatform(null);
            }

            jumpRequested = false;
            rb.linearVelocity = velocity;
        }

        void SetPlatform(MovingObject platform)
        {
            if (currentPlatform == platform) return;

            if (currentPlatform != null) currentPlatform.ReturnToStart();

            currentPlatform = platform;
        }

        public void Respawn()
        {
            SetPlatform(null);

            rb.position = checkpoint != null ? (Vector2)checkpoint.position : startPosition;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            jumpRequested = false;
            jumping = false;

            Debug.Log("낙하했습니다. 체크포인트로 복귀합니다.");
        }
    }
}