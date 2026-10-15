using UnityEngine;
using UnityEngine.AI;

namespace GM_025
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAI : MonoBehaviour
    {
        enum State
        {
            Idle,
            Chase,
            Combat
        }

        [SerializeField] private RoomData roomData;
        [SerializeField] private PlayerController player;

        State currentState;
        NavMeshAgent agent;

        int enemyHP;
        int attackDamage;

        float detectionRange;
        float attackRange;
        float interval;
        float timer = 0f;

        public event System.Action onDeath;

        public void Initialize(RoomData data, PlayerController target)
        {
            roomData = data;
            player = target;
        }

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
            detectionRange = roomData.detectionRange;
            attackRange = roomData.attackRange;
            interval = roomData.attackInterval;

            transform.localScale = Vector3.one * roomData.enemyScale;

            agent.speed = roomData.moveSpeed;
            agent.radius = roomData.agentRadius;
            agent.height = roomData.agentHeight;
            agent.baseOffset = roomData.agentBaseOffset;
            agent.stoppingDistance = attackRange * 0.9f;
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

            float distance = Mathf.Infinity;
            Vector3 targetPosition = transform.position;

            if (player != null)
            {
                targetPosition = player.transform.position;
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
                    else if (distance <= attackRange && CanAttack())
                    {
                        ChangeState(State.Combat);
                    }
                    else
                    {
                        agent.SetDestination(player.transform.position);
                    }
                    break;

                case State.Combat:
                    timer += Time.deltaTime;

                    if (timer < interval) break;

                    if (distance > detectionRange)
                    {
                        ChangeState(State.Idle);
                    }
                    else if (distance > attackRange || !CanAttack())
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

            agent.isStopped = currentState != State.Chase;

            if (currentState == State.Combat) Attack();
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
    }
}