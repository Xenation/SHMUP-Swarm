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

        public bool testController;


        //Controller
        public bool playerIndexSet = false;
        private PlayerIndex pIndex;
        private GamePadState state;
        private GamePadState prevState;

        private float vibStrengthNowRight;
        private float vibStrengthNowLeft;

        /***************
         * List of all vibrations
         * X = startTime
         * Y = duration
         * Z = strength
         * 
        ***************/
        private static List<Vector3> vibrationListLeft = new List<Vector3>();
        private static List<Vector3> vibrationListRight = new List<Vector3>();

        // Start is called before the first frame update
        void Start()
        {
            vibrationListLeft.Clear();
            vibrationListRight.Clear();
        
        }

        // Update is called once per frame
        void FixedUpdate()
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

            //Left motor
            if(vibrationListLeft.Count > 0)
            {
                float maxStrength = 0.0f;
                foreach(Vector3 vib in vibrationListLeft)
                {
                    if(Time.time > vib.x + vib.y)
                    {
                        vibrationListLeft.Remove(vib);
                    }
                    else
                    {
                        if (vib.z > maxStrength)
                            maxStrength = vib.z;
                    }
                }

                vibStrengthNowLeft = maxStrength;
            }
            else
            {
                vibStrengthNowLeft = 0.0f;
            }

            //Right motor
            if(vibrationListRight.Count > 0)
            {
                float maxStrength = 0.0f;

                //V1
                for (int i = 0; i < vibrationListRight.Count; i++)
                {
                    if (Time.fixedTime > vibrationListRight[i].x + vibrationListRight[i].y)
                    {
                        Debug.Log(Time.fixedTime);
                        Debug.Log( i + " - " + vibrationListRight[i].x + vibrationListRight[i].y);
                        //vibrationListRight.Remove(vib);
                    }
                    else
                    {
                        if (vibrationListRight[i].z > maxStrength)
                            Debug.Log("ddd");
                            maxStrength = vibrationListRight[i].z;
                    }
                }

                // V2
                /*
                foreach (Vector3 vib in vibrationListRight)
                {
                    if (Time.time > vib.x + vib.y)
                    {
                        //vibrationListRight.Remove(vib);
                    }
                    else
                    {
                        if (vib.z > maxStrength)
                            maxStrength = vib.z;
                    }
                }
                */
                vibStrengthNowRight = maxStrength;
            }
            else
            {
                vibStrengthNowRight = 0.0f;
            }

            //Testing controller motors
            if (testController)
                controllerTester();
            else
                GamePad.SetVibration(pIndex, vibStrengthNowLeft, vibStrengthNowRight);

        }

        public static void AddVibrateRight(float vibStrength, float vibDuration)
        {
            vibrationListRight.Add(new Vector3(Time.time, vibDuration, vibStrength));
        }

        public static void AddVibrateLeft(float vibStrength, float vibDuration)
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
            GamePad.SetVibration(pIndex, 0, 0);
        }
    }
}

