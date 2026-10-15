using GM_026;
using UnityEngine;

namespace GM_26
{
    public class WorldItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private string itemName;
        [SerializeField] private ItemType itemType = ItemType.None;
        [SerializeField] private KeyType keyType = KeyType.None;

        bool isCollected = false;

        public void Interact(PlayerInteraction player)
        {
            if (isCollected) return;

            if (itemType == ItemType.None || (itemType == ItemType.Key && keyType == KeyType.None))
            {
                Debug.LogWarning("아이템 종류와 결정 색상을 확인하세요.");
                return;
            }

            PlayerInventory inventory = player.GetComponent<PlayerInventory>();

            if (inventory == null)
            {
                Debug.LogWarning("플레이어에 PlayerInventory가 없습니다.");
                return;
            }

            inventory.AddItem(itemName, itemType, keyType);

            isCollected = true;
            Debug.Log($"{itemName}을 획득했습니다!");

            Destroy(gameObject);
        }

        public string GetInteractionDescription()
        {
            return $"{itemName} 줍기";
        }
    }
}
