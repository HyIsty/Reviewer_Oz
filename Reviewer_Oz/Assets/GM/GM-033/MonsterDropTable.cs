using UnityEngine;
using GM_025;

namespace GM_033
{
    [RequireComponent(typeof(EnemyAI))]
    public class MonsterDropTable : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private EnemyAI enemy;
        [SerializeField] private LootManager lootManager;

        [Header("몬스터 ID")]
        [SerializeField] private string monsterId;

        private bool onceAct;

        void Awake()
        {
            if (enemy == null) enemy = GetComponent<EnemyAI>();
            if (lootManager == null) lootManager = FindFirstObjectByType<LootManager>();
        }

        void OnEnable()
        {
            onceAct = false;

            if (enemy == null)
            {
                Debug.LogWarning("[MonsterDropTable] EnemyAI가 없습니다.");
                return;
            }

            enemy.onDeath += OnDiedDrops;
        }

        void OnDisable()
        {
            if (enemy != null) enemy.onDeath -= OnDiedDrops;
        }

        void OnDiedDrops()
        {
            if (onceAct) return;

            onceAct = true;

            if (lootManager == null)
            {
                Debug.LogWarning("[MonsterDropTable] LootManager가 없습니다.");
                return;
            }

            lootManager.OnMonsterDeath(monsterId, transform.position);
        }
    }
}