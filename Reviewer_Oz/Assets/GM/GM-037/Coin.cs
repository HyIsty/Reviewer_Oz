using UnityEngine;

namespace GM_037
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private CoinStage stage;

        bool collected = false;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (collected || other.GetComponentInParent<PlayerController>() == null) return;

            if (stage == null)
            {
                Debug.LogWarning("CoinStage를 연결하세요.");
                return;
            }

            collected = true;
            stage.CollectCoin();

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}