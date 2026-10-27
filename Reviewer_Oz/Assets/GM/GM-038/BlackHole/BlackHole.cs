using UnityEngine;

namespace GM_038
{
    public class BlackHole : MonoBehaviour
    {
        [SerializeField] private float pullRadius = 6f;
        [SerializeField] private float pullForce = 4f;
        [SerializeField] private float absorbRadius = 0.5f;

        int absorbedCount = 0;

        void FixedUpdate()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pullRadius);

            for (int i = 0; i < hits.Length; i++)
            {
                Rigidbody2D rb = hits[i].attachedRigidbody;

                if (rb == null || rb.bodyType != RigidbodyType2D.Dynamic) continue;
                if (!rb.gameObject.activeInHierarchy) continue;

                Vector2 direction = (Vector2)transform.position - rb.position;
                float distance = direction.magnitude;

                if (distance <= absorbRadius)
                {
                    rb.gameObject.SetActive(false);
                    Destroy(rb.gameObject);

                    absorbedCount++;
                    Debug.Log($"¿ÀºêÁ§Æ® Èí¼ö! ÃÑ {absorbedCount}°³");
                    continue;
                }

                rb.AddForce(direction.normalized * pullForce, ForceMode2D.Force);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, pullRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, absorbRadius);
        }
    }
}