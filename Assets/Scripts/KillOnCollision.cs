using UnityEngine;

namespace Swarm {
	public class KillOnCollision : MonoBehaviour
    {

        private CircleCollider2D bossCollider;

        private void Start()
        {
            bossCollider = GetComponent<CircleCollider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            PlayerUnit pu = collision.gameObject.GetComponent<PlayerUnit>();

            if (pu)
            {
                Destroy(collision.gameObject);
            }
        }

    }
}
