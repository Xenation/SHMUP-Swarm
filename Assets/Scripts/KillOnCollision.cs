using UnityEngine;

namespace Swarm {
	public class KillOnCollision : MonoBehaviour
    {
        
        public float vD = 1.0f;
        public float vS = 2.0f;

        private CircleCollider2D bossCollider;

        private void Start()
        {
            bossCollider = GetComponent<CircleCollider2D>();
            //Debug.Log("Duration : " + vD + "Str = " + vS);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            PlayerUnit pu = collision.gameObject.GetComponent<PlayerUnit>();

            if (pu)
            {
                AkSoundEngine.PostEvent("Play_Death", gameObject);

                //Debug.Log("Duration : " + vD + "Str = " + vS);
                VibrationManager.AddVibrateRight(vS, vD);
                Destroy(collision.gameObject);
            }
        }
       
    }
}
