using UnityEngine;

namespace Swarm {
	public class KillOnCollision : MonoBehaviour
    {

        public float vibDuration = 1.0f;
        public float vibStrength = 2.5f;


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
                AkSoundEngine.PostEvent("Play_Death", gameObject);
                VibrationManager.AddVibrateRight(vibStrength, vibDuration);
                Destroy(collision.gameObject);
            }
        }
       
    }
}
