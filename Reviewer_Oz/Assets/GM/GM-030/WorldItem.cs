using UnityEngine;
using GM_026;

namespace GM_030
{
    public class WorldItem : MonoBehaviour
    {
        public string itemId;
        public string itemDisplayName;
        public int amount = 1;

        public ItemType itemType = ItemType.None;
        public KeyType keyType = KeyType.None;

        public WorldItem(string itemId, string itemDisplayName, int amount)
        {
            this.itemId = itemId;
            this.itemDisplayName = itemDisplayName;
            this.amount = amount;
        }
    }

}