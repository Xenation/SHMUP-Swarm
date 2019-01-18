using UnityEngine;

namespace Swarm {
	public class KillOnCollision : MonoBehaviour
    {
        
        public float vD = 1.0f;
        public float vS = 2.0f;
        public bool lazer = false;
        public int dmgPerSecond = 5;
        private float firstHitTime = 0;

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
				pu.Die(vD, vS);
                ScoreManager.nbPyuKilled++;
            }

            PlayerShrink ps = collision.gameObject.GetComponent<PlayerShrink>();

            if (ps)
            {
                if (lazer)
                {
                    firstHitTime = Time.time;
                }
                else
                {
                    ps.Die(vD, vS);
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            PlayerShrink ps = collision.gameObject.GetComponent<PlayerShrink>();

            if (ps)
            {
                if (lazer && Time.time > (firstHitTime + (1.0f / (float)dmgPerSecond)))
                {
                    Debug.Log("ouch " + Time.time);
                    ps.Die(vD, vS);
                    firstHitTime = Time.time;
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerUnit pu = collision.gameObject.GetComponent<PlayerUnit>();

            if (pu)
            {
                pu.Die(vD, vS);
            }

            PlayerShrink ps = collision.gameObject.GetComponent<PlayerShrink>();

            if (ps)
            {
                if (lazer)
                {
                    firstHitTime = Time.time;
                }
                else
                {
                    ps.Die(vD, vS);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            PlayerShrink ps = collision.gameObject.GetComponent<PlayerShrink>();

            if (ps)
            {
                if (lazer && Time.time > (firstHitTime + (1.0f / (float)dmgPerSecond)))
                {
                    Debug.Log("ouch " + Time.time);
                    ps.Die(vD, vS);
                    firstHitTime = Time.time;
                }
            }
        }

    }
}
