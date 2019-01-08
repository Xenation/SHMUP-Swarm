using UnityEngine;
using XInputDotNetPure;

namespace Swarm {
	public class KillOnCollision : MonoBehaviour
    {

        private CircleCollider2D bossCollider;
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
        }

        private void FixedUpdate()
        {
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
                GamePad.SetVibration(pIndex, vibrationStrength, vibrationStrength);
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
    }
}
