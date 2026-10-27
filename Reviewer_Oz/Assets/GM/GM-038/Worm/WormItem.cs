using TMPro;
using UnityEngine;

namespace GM_038
{
    public class WormItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text numberText;

        public int Number { get; private set; }
        public float Radius => GetComponent<SpriteRenderer>().bounds.extents.x;

        public void Initialize(int number)
        {
            Number = number;
            numberText.text = number.ToString();
        }

        public void Collect()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}