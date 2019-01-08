using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace Swarm
{
    public class VibrationManager : MonoBehaviour
    {

        public PlayerSwarm swarm;

        public Boss boss;


        //Controller
        public static bool playerIndexSet = false;
        private static PlayerIndex pIndex;
        private static GamePadState state;
        private static GamePadState prevState;


        private static float vibStartTime = 0;
        public static float vibDuration = 0.1f;
        public static float vibStrength = 1.0f;

        private static float vibStrengthNowRight;
        private static float vibStrengthNowLeft;

        public static float vibDurationLeft;
        public static float vibDurationRight;

        private static float vibStartTimeLeft;
        private static float vibStartTimeRight;

        /***************
         * X = startTime
         * Y = duration
         * Z = strength
         * 
        ***************/
        private static List<Vector3> vibrationListLeft= new List<Vector3>();
        private static List<Vector3> vibrationListRight= new List<Vector3>();

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Testing controller motors
            if (swarm.testController)
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


            if(vibrationListLeft.Count > 0)
            {

            }

            if(vibrationListRight.Count > 0)
            {

            }

            //To stop vibrations
            if (Time.time - vibStartTime >= vibDuration)
                GamePad.SetVibration(pIndex, 0, 0);
        }

        public static void AddVibrateRight(float vibStrength, float vibDuration)
        {
            vibrationListRight.Add(new Vector3(Time.time, vibDuration, vibStrength));
        }

        public static void VibrateLeft(float vibStrength, float vibDuration)
        {
            vibrationListLeft.Add(new Vector3(Time.time, vibDuration, vibStrength));
        }

        public static void StopVibRight()
        {
            vibrationListRight.Clear();
        }

        public static void StopVibLeft()
        {
            vibrationListLeft.Clear();
        }


        public static void StopVibration()
        {
            StopVibRight();
            StopVibLeft();
        }

        private void controllerTester()
        {
            GamePad.SetVibration(pIndex, -swarm.cursor.position.x / 10, swarm.cursor.position.x / 10);
        }

        private void OnDestroy()
        {
            StopVibration();
        }
    }
}

