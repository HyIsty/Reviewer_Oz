using UnityEngine;

namespace GM_038
{
    public class RacingEnemy : MonoBehaviour
    {
        [SerializeField] private float driveSpeed = 2f;
        [SerializeField] private int passScore = 20;
        [SerializeField] private int destroyScore = 100;
        [SerializeField] private float despawnY = -9f;

        RacingStage stage;
        Rigidbody2D rb;

        bool scored = false;

        public bool IsResolved { get; private set; }

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(RacingStage racingStage)
        {
            stage = racingStage;
        }

        void FixedUpdate()
        {
            if (stage == null || stage.IsGameOver || IsResolved) return;

            float speed = driveSpeed + stage.ScrollSpeed;
            Vector2 nextPosition = rb.position + Vector2.down * speed * Time.fixedDeltaTime;

            rb.MovePosition(nextPosition);

            if (!scored && nextPosition.y < stage.PassLine)
            {
                scored = true;
                stage.AddScore(passScore);

                Debug.Log($"차량 통과! +{passScore}");
            }

            if (nextPosition.y < despawnY)
            {
                IsResolved = true;
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }

        public void DestroyEnemy()
        {
            if (stage == null || stage.IsGameOver || IsResolved) return;

            IsResolved = true;

            if (!scored)
            {
                scored = true;
                stage.AddScore(destroyScore);

                Debug.Log($"차량 파괴! +{destroyScore}");
            }

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}