using UnityEngine;


namespace GM_024
{
    public class EnemyAI : MonoBehaviour
    {
        enum State
        {
            Idle,
            Chase,
            Combat
        }

        [SerializeField] private Transform player;
        [SerializeField] private float detectionRange = 8f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float interval = 2f;

        State currentState;
        Animator animator;
        float timer = 0f;

        void Start()
        {
            animator = GetComponent<Animator>();

            if (animator == null) 
                Debug.LogWarning("Animator가 없어 애니메이션을 재생할 수 없습니다.");

            ChangeState(State.Idle);
        }

        void Update()
        {
            float distance = Mathf.Infinity;
            Vector3 targetPosition = transform.position;

            if (player != null)
            {
                targetPosition = player.position;
                targetPosition.y = transform.position.y;
                distance = Vector3.Distance(transform.position, targetPosition);

                if (currentState != State.Idle)
                {
                    Vector3 direction = targetPosition - transform.position;

                    if (direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(direction);
                }
            }

            switch (currentState)
            {
                case State.Idle:
                    if (distance <= detectionRange) ChangeState(State.Chase);
                    break;

                case State.Chase:
                    if (distance > detectionRange)
                    {
                        ChangeState(State.Idle);
                    }
                    else if (distance <= attackRange)
                    {
                        ChangeState(State.Combat);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                    }
                    break;

                case State.Combat:
                    timer += Time.deltaTime;

                    if (timer < interval) break;

                    if (distance > detectionRange)
                    {
                        ChangeState(State.Idle);
                    }
                    else if (distance > attackRange)
                    {
                        ChangeState(State.Chase);
                    }
                    else
                    {
                        ChangeState(State.Combat);
                    }
                    break;
            }
        }

        void ChangeState(State state)
        {
            currentState = state;
            timer = 0f;

            if (animator != null)
            {
                animator.SetInteger("State", (int)currentState);
                animator.ResetTrigger("Attack");

                if (currentState == State.Combat) 
                    animator.SetTrigger("Attack");
            }

            if (currentState == State.Combat)
                Attack();
        }

        void Attack()
        {
            Debug.Log("적이 플레이어를 공격합니다!");
        }

    }

}
