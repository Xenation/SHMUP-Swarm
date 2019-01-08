using UnityEngine;
using XInputDotNetPure;

namespace Swarm {
	public class KillOnCollision : MonoBehaviour
    {

        private CircleCollider2D bossCollider;
        private GameObject player;
        private PlayerSwarm swarm;

        //Controller
        bool playerIndexSet = false;
        private PlayerIndex pIndex;
        private GamePadState state;
        private GamePadState prevState;
        private float startVib = 0;
        public float vibrationDuration = 0.1f;
        public float vibrationStrength = 1.0f;


        private void Start()
        {
            bossCollider = GetComponent<CircleCollider2D>();

            player = GameObject.Find("PlayerSwarm");
            swarm = player.GetComponent<PlayerSwarm>();
            
        }

        private void FixedUpdate()
        {
            if(swarm.testController)
                controllerTester();

            //Controller
            if (!playerIndexSet || !prevState.IsConnected)
            {
                for (int i = 0; i < 4; ++i)
                {
                    PlayerIndex testPlayerIndex = (PlayerIndex)i;
                    GamePadState testState = GamePad.GetState(testPlayerIndex);
                    if (testState.IsConnected)
                    {
                        pIndex = testPlayerIndex;
                        playerIndexSet = true;
                    }
                }
            }

            prevState = state;
            state = GamePad.GetState(pIndex);
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

        private void StopVibration()
        {
            if(Time.time - startVib >= vibrationDuration)
                GamePad.SetVibration(pIndex, 0, 0);
        }


        private void controllerTester()
        {
            GamePad.SetVibration(pIndex, - swarm.cursor.position.x / 10, swarm.cursor.position.x / 10);
        }
    }
}
