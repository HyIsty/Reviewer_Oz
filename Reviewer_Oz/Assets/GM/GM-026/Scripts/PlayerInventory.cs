using UnityEngine;
using System.Collections.Generic;

namespace GM_026
{
    [System.Serializable]
    public struct InventoryItem
    {
        public string itemName;
        public ItemType itemType;
        public KeyType keyType;
    }

    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();

        public void AddItem(string itemName, ItemType itemType, KeyType keyType)
        {
            InventoryItem item = new InventoryItem();
            item.itemName = itemName;
            item.itemType = itemType;
            item.keyType = keyType;

            items.Add(item);
        }

        public bool HasKey(KeyType keyType)
        {
            if (keyType == KeyType.None) return false;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemType == ItemType.Key && items[i].keyType == keyType)
                {
                    return true;
                }
            }

            return false;
        }

        public void UseKey(KeyType keyType)
        {
            if (keyType == KeyType.None) return;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemType != ItemType.Key || items[i].keyType != keyType) continue;

                Debug.Log($"{items[i].itemName}을 소모했습니다.");
                items.RemoveAt(i);
                return;
            }

            Debug.LogWarning($"{keyType} 결정이 없습니다.");
        }
    }
}