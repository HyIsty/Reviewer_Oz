using UnityEngine;
using UnityEngine.AI;
using GM_025;
using System;

namespace GM_034
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAI : MonoBehaviour
    {
        enum State
        {
            Idle,
            Chase,
            Attack
        }

        [Header("참조")]
        [SerializeField] private RoomData roomData;
        [SerializeField] private PlayerController player;

        [Header("시야")]
        [SerializeField, Range(0f, 360f)] private float viewAngle = 90f;

        [Header("현재 상태")]
        [SerializeField] private State currentState;

        NavMeshAgent agent;

        int enemyHP;
        int attackDamage;

        float chaseDistance;
        float attackDistance;
        float interval;
        float timer = 0f;

        public event Action onDeath;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            if (roomData == null || player == null)
            {
                Debug.LogWarning("RoomData와 Player를 연결하세요.");
                enabled = false;
                return;
            }

            enemyHP = roomData.maxHP;
            attackDamage = roomData.attackDamage;
            chaseDistance = roomData.detectionRange;
            attackDistance = roomData.attackRange;
            interval = roomData.attackInterval;

            transform.localScale = Vector3.one * roomData.enemyScale;

            agent.speed = roomData.moveSpeed;
            agent.radius = roomData.agentRadius;
            agent.height = roomData.agentHeight;
            agent.baseOffset = roomData.agentBaseOffset;
            agent.stoppingDistance = attackDistance * 0.9f;
            agent.updateRotation = false;

            if (!agent.isOnNavMesh)
            {
                Debug.LogWarning("적이 NavMesh 위에 없습니다. Bake와 Agent Type을 확인하세요.");
                enabled = false;
                return;
            }

            ChangeState(State.Idle);
        }

        void Update()
        {
            if (enemyHP <= 0 || !agent.isOnNavMesh) return;

            if (player == null)
            {
                if (currentState != State.Idle) ChangeState(State.Idle);
                return;
            }

            Vector3 targetPosition = player.transform.position;
            targetPosition.y = transform.position.y;

            float distance = Vector3.Distance(transform.position, targetPosition);

            if (currentState != State.Idle)
            {
                Vector3 direction = targetPosition - transform.position;

                if (direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(direction);
            }

            switch (currentState)
            {
                case State.Idle:
                    Vector2 enemyPosition = new Vector2(transform.position.x, transform.position.z);
                    Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);
                    Vector2 playerPosition = new Vector2(targetPosition.x, targetPosition.z);

                    if (IsTargetInsideViewCone(enemyPosition, forward, playerPosition, chaseDistance, viewAngle))
                    {
                        ChangeState(State.Chase);
                    }
                    break;

                case State.Chase:
                    if (distance > chaseDistance * 1.2f)
                    {
                        ChangeState(State.Idle);
                    }
                    else if (distance <= attackDistance && CanAttack())
                    {
                        ChangeState(State.Attack);
                    }
                    else
                    {
                        agent.SetDestination(targetPosition);
                    }
                    break;

                case State.Attack:
                    if (distance > attackDistance || !CanAttack())
                    {
                        ChangeState(State.Chase);
                        break;
                    }

                    timer += Time.deltaTime;

                    if (timer >= interval) ChangeState(State.Attack);
                    break;
            }
        }

        void ChangeState(State state)
        {
            if (currentState != state)
            {
                Debug.Log($"적 상태 변경 : {currentState} -> {state}");
            }

            currentState = state;
            timer = 0f;

            agent.isStopped = currentState != State.Chase;

            if (currentState == State.Attack) Attack();
        }

        bool IsTargetInsideViewCone(Vector2 enemyPosition, Vector2 forward, Vector2 targetPosition, float viewDistance, float viewAngle)
        {
            if (viewDistance < 0f || forward.sqrMagnitude == 0f) return false;

            Vector2 direction = targetPosition - enemyPosition;
            float distance = direction.magnitude;

            if (distance > viewDistance) return false;

            if (distance == 0f) return true;

            forward.Normalize();
            direction.Normalize();

            float halfAngle = Mathf.Clamp(viewAngle, 0f, 360f) * 0.5f;
            float minimumDot = Mathf.Cos(halfAngle * Mathf.Deg2Rad);
            float dot = Vector2.Dot(forward, direction);

            return dot >= minimumDot;
        }

        bool CanAttack()
        {
            if (player == null) return false;

            if (Physics.Linecast(transform.position, player.transform.position, out RaycastHit hit, ~0, QueryTriggerInteraction.Ignore))
            {
                return hit.collider.GetComponentInParent<PlayerController>() == player;
            }

            return false;
        }

        void Attack()
        {
            if (player == null) return;

            player.TakeDamage(attackDamage);
            Debug.Log($"적이 플레이어를 공격합니다! 공격력 : {attackDamage}");
        }

        public void TakeDamage(int damage)
        {
            if (enemyHP <= 0 || damage <= 0) return;

            enemyHP = Mathf.Max(0, enemyHP - damage);
            Debug.Log($"적이 {damage} 데미지를 받았습니다. 현재 HP : {enemyHP}");

            if (enemyHP <= 0) Die();
        }

        void Die()
        {
            if (agent.isOnNavMesh) agent.isStopped = true;

            Debug.Log("적이 사망했습니다.");

            if (onDeath != null) onDeath();

            Destroy(gameObject);
        }

        void OnDrawGizmosSelected()
        {
            if (roomData == null) return;

            float attackRadius = roomData.attackRange;
            float chaseRadius = roomData.detectionRange;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadius * 1.2f);

            if (player != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(transform.position, player.transform.position);
            }

            DrawViewCone(chaseRadius);
        }

        void DrawViewCone(float viewDistance)
        {
            Vector3 forward = transform.forward;
            forward.y = 0f;

            if (forward.sqrMagnitude == 0f) return;

            forward.Normalize();

            float halfAngle = Mathf.Clamp(viewAngle, 0f, 360f) * 0.5f;

            Vector3 leftDirection = Quaternion.Euler(0f, -halfAngle, 0f) * forward;
            Vector3 rightDirection = Quaternion.Euler(0f, halfAngle, 0f) * forward;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + leftDirection * viewDistance);
            Gizmos.DrawLine(transform.position, transform.position + rightDirection * viewDistance);

            Vector3 previousPosition = transform.position + leftDirection * viewDistance;

            for (int i = 1; i <= 30; i++)
            {
                float angle = Mathf.Lerp(-halfAngle, halfAngle, i / 30f);
                Vector3 direction = Quaternion.Euler(0f, angle, 0f) * forward;
                Vector3 nextPosition = transform.position + direction * viewDistance;

                Gizmos.DrawLine(previousPosition, nextPosition);
                previousPosition = nextPosition;
            }
        }
    }
}