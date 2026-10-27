using TMPro;
using UnityEngine;

namespace GM_038
{
    public class WormSegment : MonoBehaviour
    {
        [SerializeField] private TMP_Text numberText;
        [SerializeField] private float growthStep = 0.1f;

        SpriteRenderer sprite;
        Vector3 originalScale;
        float growth = 1f;

        public float Radius => Mathf.Max(sprite.bounds.extents.x, sprite.bounds.extents.y);

        void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            originalScale = transform.localScale;
        }

        public void Initialize(int number, Color color)
        {
            numberText.text = number.ToString();
            sprite.color = color;
        }

        public void Grow()
        {
            growth += growthStep;
            transform.localScale = originalScale * growth;
        }
    }
}