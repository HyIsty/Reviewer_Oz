using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GM_030
{
    public class PlayerInventoryDictionary : MonoBehaviour
    {
        [Header("등록된 아이템 ID")]
        [SerializeField]
        private string[] validItemIDs = { "potion", "key_red", "key_blue", "key_green", "gold", "laevateinn", "sword", "tree_of_life" };

        [Header("참조")]
        [SerializeField] private ShopNpcDictionary npc;

        private readonly Dictionary<string, int> itemCountById = new Dictionary<string, int>();
        private readonly Dictionary<string, int> laevateinnCrafter = new Dictionary<string, int>();

        void Awake()
        {
            laevateinnCrafter.Add("sword", 1);
            laevateinnCrafter.Add("tree_of_life", 1);

            AddItem("sword", 2);
            AddItem("potion", 1);
            AddItem("gold", 100);
            AddItem("key_red", 3);
            AddItem("tree_of_life", 2);
        }

        public void OnInventoryCheck(InputValue value)
        {
            if (!value.isPressed) return;

            PrintInventory();
        }

        public void OnCheckShop(InputValue value)
        {
            if (!value.isPressed || npc == null) return;

            npc.ShopItemText();
        }

        public void OnCheckShopItem1(InputValue value)
        {
            if (!value.isPressed || npc == null) return;

            ShopItemPriceText("potion");
        }

        public void OnCheckShopItem2(InputValue value)
        {
            if (!value.isPressed || npc == null) return;

            ShopItemPriceText("sword");
        }

        public void OnCraftItem(InputValue value)
        {
            if (!value.isPressed || npc == null) return;

            npc.CraftItem("laevateinn", laevateinnCrafter, this);
        }

        bool IsValidItemId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;

            for (int i = 0; i < validItemIDs.Length; i++)
            {
                if (validItemIDs[i] == itemId)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetItemCount(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return 0;

            if (itemCountById.TryGetValue(itemId, out int count))
            {
                return count;
            }

            return 0;
        }

        public bool CanAddItem(string itemId, int amount)
        {
            if (amount <= 0) return false;
            if (!IsValidItemId(itemId)) return false;

            return true;
        }

        public bool AddItem(WorldItem item)
        {
            if (item == null) return false;

            if (!AddItem(item.itemId, item.amount))
            {
                return false;
            }

            Debug.Log($"{item.itemDisplayName} {item.amount}개를 획득했습니다.");

            return true;
        }

        public bool AddItem(string itemId, int amount)
        {
            if (!CanAddItem(itemId, amount))
            {
                Debug.LogWarning($"아이템을 추가할 수 없습니다. ID : {itemId} | 수량 : {amount}");
                return false;
            }

            IncreaseItemCount(itemId, amount);
            PrintInventory();

            return true;
        }

        public bool ConsumeItem(string itemId, int amount)
        {
            if (string.IsNullOrEmpty(itemId) || amount <= 0) return false;
            if (GetItemCount(itemId) < amount) return false;

            DecreaseItemCount(itemId, amount);

            return true;
        }

        private void IncreaseItemCount(string itemId, int amount)
        {
            if (itemCountById.TryGetValue(itemId, out int currentCount))
            {
                itemCountById[itemId] = currentCount + amount;
            }
            else
            {
                itemCountById.Add(itemId, amount);
            }
        }

        private void DecreaseItemCount(string itemId, int amount)
        {
            if (!itemCountById.TryGetValue(itemId, out int currentCount)) return;

            int nextCount = currentCount - amount;

            if (nextCount <= 0)
            {
                itemCountById.Remove(itemId);
            }
            else
            {
                itemCountById[itemId] = nextCount;
            }
        }

        public void PrintInventory()
        {
            string log = "==== INVENTORY ====\n";

            foreach (KeyValuePair<string, int> pair in itemCountById)
            {
                log += $"{pair.Key} : {pair.Value}\n";
            }

            Debug.Log(log);
        }

        void ShopItemPriceText(string itemId)
        {
            int price = npc.GetPrice(itemId);

            if (price > 0)
            {
                Debug.Log($"[Shop] {itemId} : {price}G");
            }
            else
            {
                Debug.Log($"상점에 등록되지 않은 아이템입니다. : {itemId}");
            }
        }
    }

}