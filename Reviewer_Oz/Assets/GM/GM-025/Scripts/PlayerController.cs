using UnityEngine;
using UnityEngine.InputSystem;

namespace GM_025
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;

        [SerializeField] private int maxHP = 100;
        [SerializeField] private int attackDamage = 10;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackInterval = 0.5f;

        CharacterController controller;
        Vector2 moveInput;

        private int playerHP;
        private float attackTimer = 0f;

        void Awake()
        {
            controller = this.GetComponent<CharacterController>();
            playerHP = maxHP;
            attackTimer = attackInterval;
        }

        void Update()
        {
            if (playerHP <= 0) return;

            attackTimer += Time.deltaTime;

            Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
            direction = Vector3.ClampMagnitude(direction, 1f);

            if (direction != Vector3.zero) transform.forward = direction;

            controller.Move(direction * moveSpeed * Time.deltaTime);
        }

        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        public void OnAttack(InputValue value)
        {
            if (!value.isPressed || playerHP <= 0) return;

            Attack();
        }

        void Attack()
        {
            if (attackTimer < attackInterval) return;

            attackTimer = 0f;

            Vector3 origin = controller.bounds.center;
            Debug.DrawRay(origin, transform.forward * attackRange, Color.red, 0.2f);

            if (!Physics.Raycast(origin, transform.forward, out RaycastHit hit, attackRange, ~0, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("공격 범위에 대상이 없습니다.");
                return;
            }

            EnemyAI enemy = hit.collider.GetComponentInParent<EnemyAI>();

            if (enemy == null) return;

            enemy.TakeDamage(attackDamage);
        }

        public void TakeDamage(int damage)
        {
            if (playerHP <= 0 || damage <= 0) return;

            playerHP = Mathf.Max(0, playerHP - damage);
            Debug.Log($"플레이어가 {damage} 데미지를 받았습니다. 현재 HP : {playerHP}");

            if (playerHP <= 0)
            {
                Debug.Log("플레이어가 사망했습니다.");
            }
        }


    }

}
