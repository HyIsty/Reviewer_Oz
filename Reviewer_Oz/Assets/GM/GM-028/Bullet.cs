using UnityEngine;

namespace GM_028
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float lifetime = 3f;
        [SerializeField] private float speed = 5f;

        void Start()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.linearVelocity = transform.forward * speed;

            Destroy(gameObject, lifetime);
        }
    }
}