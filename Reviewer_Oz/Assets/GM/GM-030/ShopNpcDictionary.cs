using System.Collections.Generic;
using UnityEngine;

namespace GM_030
{
    public class ShopNpcDictionary : MonoBehaviour
    {
        private readonly Dictionary<string, int> itemPrices = new Dictionary<string, int>();

        void Awake()
        {
            itemPrices.Clear();

            AddValue("potion", 50);
            AddValue("sword", 300);
        }

        void AddValue(string itemId, int price)
        {
            if (!itemPrices.ContainsKey(itemId))
            {
                itemPrices.Add(itemId, price);
            }
        }

        public int GetPrice(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return 0;

            if (itemPrices.TryGetValue(itemId, out int price))
            {
                return price;
            }

            return 0;
        }

        public void ShopItemText()
        {
            string log = "==== SHOP ====\n";

            foreach (KeyValuePair<string, int> pair in itemPrices)
            {
                log += $"{pair.Key} - {pair.Value}G\n";
            }

            Debug.Log(log);
        }

        public bool CraftItem(string recipeId, Dictionary<string, int> requiredMaterials, PlayerInventoryDictionary inventory)
        {
            if (inventory == null)
            {
                Debug.LogWarning("인벤토리가 연결되지 않았습니다.");
                return false;
            }

            if (requiredMaterials == null || requiredMaterials.Count == 0)
            {
                Debug.LogWarning("조합 재료 정보가 없습니다.");
                return false;
            }

            if (!inventory.CanAddItem(recipeId, 1))
            {
                Debug.LogWarning($"등록되지 않은 조합 결과물입니다. : {recipeId}");
                return false;
            }

            foreach (KeyValuePair<string, int> pair in requiredMaterials)
            {
                if (pair.Value <= 0)
                {
                    Debug.LogWarning($"잘못된 재료 수량입니다. : {pair.Key} / {pair.Value}");
                    return false;
                }

                int currentCount = inventory.GetItemCount(pair.Key);

                if (currentCount < pair.Value)
                {
                    Debug.Log($"{pair.Key}의 개수가 부족합니다. 보유 : {currentCount} / 필요 : {pair.Value}");
                    return false;
                }
            }

            foreach (KeyValuePair<string, int> pair in requiredMaterials)
            {
                inventory.ConsumeItem(pair.Key, pair.Value);
            }

            inventory.AddItem(recipeId, 1);

            Debug.Log($"{recipeId}를 조합하여 만들었습니다.");

            return true;
        }
    }
}