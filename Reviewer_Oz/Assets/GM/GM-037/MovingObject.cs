using UnityEngine;

namespace GM_037
{
    public class MovingObject : MonoBehaviour
    {
        [SerializeField] private Vector2 moveOffset = new Vector2(5f, 0f);
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float returnSpeed = 4f;

        Rigidbody2D rb;
        Vector2 startPosition;
        Vector2 velocity;

        bool movingToEnd = true;
        bool returning = false;

        float lastMoveTime = -1f;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;

            startPosition = rb.position;
            moveSpeed = Mathf.Max(0.1f, moveSpeed);
            returnSpeed = Mathf.Max(0.1f, returnSpeed);
        }

        void FixedUpdate()
        {
            PrepareMovement();
        }

        public Vector2 PrepareMovement()
        {
            if (lastMoveTime == Time.fixedTime) return velocity;

            lastMoveTime = Time.fixedTime;

            Vector2 targetPosition = movingToEnd ? startPosition + moveOffset : startPosition;

            if (returning) targetPosition = startPosition;

            float speed = returning ? returnSpeed : moveSpeed;
            Vector2 nextPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);

            velocity = (nextPosition - rb.position) / Time.fixedDeltaTime;
            rb.MovePosition(nextPosition);

            if (nextPosition == targetPosition)
            {
                if (returning)
                {
                    returning = false;
                    movingToEnd = true;
                }
                else
                {
                    movingToEnd = !movingToEnd;
                }
            }

            return velocity;
        }

        public void ReturnToStart()
        {
            PrepareMovement();
            returning = true;
        }
    }
}