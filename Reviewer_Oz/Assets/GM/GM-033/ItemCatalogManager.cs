using System.Collections.Generic;
using UnityEngine;
using GM_026;


namespace GM_033
{
    [System.Serializable]
    public struct ItemData
    {
        public string itemId;
        public string displayName;
        public ItemType itemType;
        public KeyType keyType;
    }

    public class ItemCatalogManager : MonoBehaviour
    {
        [SerializeField] private ItemData[] items;

        private readonly Dictionary<string, ItemData> itemDictionary = new Dictionary<string, ItemData>();

        void Awake()
        {
            itemDictionary.Clear();

            if (items == null || items.Length == 0)
            {
                Debug.LogWarning("[ItemCatalogManager] 등록된 아이템이 없습니다.");
                return;
            }

            foreach (ItemData item in items)
            {
                if (string.IsNullOrEmpty(item.itemId))
                {
                    Debug.LogWarning("[ItemCatalogManager] 비어 있는 아이템 ID가 있습니다.");
                    continue;
                }

                if (itemDictionary.ContainsKey(item.itemId))
                {
                    Debug.LogWarning($"중복된 아이템 ID입니다. : {item.itemId}");
                    continue;
                }

                itemDictionary.Add(item.itemId, item);
            }
        }

        public bool IsRegistered(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;

            return itemDictionary.ContainsKey(itemId);
        }

        public bool TryGetItem(string itemId, out ItemData itemData)
        {
            itemData = default;

            if (string.IsNullOrEmpty(itemId)) return false;

            return itemDictionary.TryGetValue(itemId, out itemData);
        }
    }
}
