using GM_030;
using UnityEngine;

namespace GM_033
{
    [RequireComponent(typeof(PlayerInventoryDictionary))]
    public class ItemPickupCollector : MonoBehaviour
    {
        private PlayerInventoryDictionary inventory;

        void Awake()
        {
            inventory = GetComponent<PlayerInventoryDictionary>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (inventory == null) return;

            WorldItem item = other.GetComponentInParent<WorldItem>();

            if (item == null || !item.gameObject.activeInHierarchy) return;
            if (!inventory.AddItem(item)) return;

            item.gameObject.SetActive(false);
            Destroy(item.gameObject);
        }
    }
}