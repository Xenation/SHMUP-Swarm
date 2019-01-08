using UnityEngine;
using XInputDotNetPure;

namespace Swarm {
	public class KillOnCollision : MonoBehaviour
    {

        private CircleCollider2D bossCollider;
        private GameObject player;
        private PlayerSwarm swarm;



        private void Start()
        {
            bossCollider = GetComponent<CircleCollider2D>();

            player = GameObject.Find("PlayerSwarm");
            swarm = player.GetComponent<PlayerSwarm>();
            
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            PlayerUnit pu = collision.gameObject.GetComponent<PlayerUnit>();

            if (pu)
            {
                AkSoundEngine.PostEvent("Play_Death", gameObject);

                //Vector2 diff = swarm.cursor.transform.position - pu.transform.position;

                float distBetweenPointsRight = Mathf.Sqrt(Mathf.Pow(pu.transform.position.x - swarm.cursor.position.x + 0.5f, 2) + Mathf.Pow(pu.transform.position.y - swarm.cursor.position.y, 2));
                float distBetweenPointsLeft = Mathf.Sqrt(Mathf.Pow(pu.transform.position.x - swarm.cursor.position.x - 0.5f, 2) + Mathf.Pow(pu.transform.position.y - swarm.cursor.position.y, 2));

                float ratioRight = 1 - distBetweenPointsRight / 8.0f; //Divisé par le ratio;
                float ratioLeft = 1 - distBetweenPointsLeft / 8.0f;

                float vibrationStrRight = vibrationStrength * ratioRight;
                float vibrationStrLeft = vibrationStrength * ratioLeft;



                GamePad.SetVibration(pIndex, vibrationStrLeft, vibrationStrRight);

                Destroy(collision.gameObject);
                startVib = Time.time;
                Invoke("StopVibration", vibrationDuration);
            }
        }
       
    }
}
