using TMPro;
using UnityEngine;

namespace GM_037
{
    public class CoinStage : MonoBehaviour
    {
        [SerializeField] private int totalCoins = 7;
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private TMP_Text clearText;

        int remainingCoins;

        void Awake()
        {
            remainingCoins = totalCoins;
        }

        void Start()
        {
            clearText.gameObject.SetActive(false);
            RefreshUI();
        }

        public void CollectCoin()
        {
            if (remainingCoins <= 0) return;

            remainingCoins--;
            RefreshUI();

            if (remainingCoins == 0)
            {
                clearText.gameObject.SetActive(true);
                Debug.Log("스테이지 클리어!");
            }
        }

        void RefreshUI()
        {
            coinText.text = $"Remaining Coins : {remainingCoins} / {totalCoins}";
        }
    }
}