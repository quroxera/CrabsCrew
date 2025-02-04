using UnityEngine;

namespace Scripts.Components.GoBased
{
    public class MovingObjectComponent : MonoBehaviour
    {
        [SerializeField] private Vector3 pointA;
        [SerializeField] private Vector3 pointB;
        [SerializeField] public float speed;

        private Vector3 target;

        public void Start()
        {
            target = pointB;
        }

        public void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (transform.position == pointB)
            {
                target = pointA;
            }
            else if (transform.position == pointA)
            {
                target = pointB;
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.parent = transform;
            }
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.parent = null;
            }
        }
    }
}
