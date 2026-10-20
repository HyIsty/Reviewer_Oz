using GM_030;
using TMPro;
using UnityEngine;

namespace GM_031
{
    public class InventoryHUD : MonoBehaviour
    {
        [Header("ÂüÁ¶")]
        [SerializeField] private PlayerInventoryDictionary inventory;

        [Header("±âº» HUD")]
        [SerializeField] private TMP_Text potionText;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text keyText;

        [Header("ÀÎº¥Åä¸® Ã¢")]
        [SerializeField] private GameObject inventoryPanel;

        [Header("½½·Ô ÅØ½ºÆ®")]
        [SerializeField] private TMP_Text potionSlotText;
        [SerializeField] private TMP_Text goldSlotText;
        [SerializeField] private TMP_Text redKeySlotText;

        void Start()
        {
            CloseInventory();
        }

        void Update()
        {
            RefreshUI();
        }

        void RefreshUI()
        {
            if (inventory == null) return;

            int potionCount = inventory.GetItemCount("potion");
            int goldCount = inventory.GetItemCount("gold");
            int keyCount = inventory.GetItemCount("key_red");

            potionText.text = $"Æ÷¼Ç : {potionCount}";
            goldText.text = $"°ñµå : {goldCount}";
            keyText.text = $"¿­¼è : {keyCount}";

            if (inventoryPanel == null || !inventoryPanel.activeSelf) return;

            potionSlotText.text = $"Æ÷¼Ç\nx{potionCount}";
            goldSlotText.text = $"°ñµå\nx{goldCount}";
            redKeySlotText.text = $"»¡°£ ¿­¼è\nx{inventory.GetItemCount("key_red")}";
        }

        public void OpenInventory()
        {
            if (inventoryPanel == null) return;

            inventoryPanel.SetActive(true);
            RefreshUI();
        }

        public void CloseInventory()
        {
            if (inventoryPanel == null) return;

            inventoryPanel.SetActive(false);
        }
    }

}