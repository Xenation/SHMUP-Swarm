using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace Swarm
{
    public class VibrationManager : MonoBehaviour
    {

        public PlayerSwarm swarm;

        private static VibrationManager vm;

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
        private List<Vector3> vibrationListLeft = new List<Vector3>();
        private List<Vector3> vibrationListRight = new List<Vector3>();


        private void Awake()
        {
            vm = this;
        }

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
                Debug.Log("JJE");
                float maxStrength = 0.0f;
                List<Vector3> deleteIndex = new List<Vector3>();

                foreach (Vector3 vib in vibrationListLeft)
                {
                    if(Time.time > vib.x + vib.y)
                    {
                        deleteIndex.Add(vib);
                    }
                    else
                    {
                        if (vib.z > maxStrength)
                            maxStrength = vib.z;
                        Debug.Log(1323);
                    }
                }

                vibStrengthNowLeft = maxStrength;

                //Delete finished vibrations
                foreach (Vector3 v in deleteIndex)
                {
                    vibrationListLeft.Remove(v);
                }
                deleteIndex.Clear();
            }
            else
            {
                vibStrengthNowLeft = 0.0f;
            }

            

            //Right motor
            if(vibrationListRight.Count > 0)
            {
                Debug.Log("JJR");
                float maxStrength = 0.0f;
                List<Vector3> deleteIndex = new List<Vector3>();

                //V1
                
                for (int i = 0; i < vibrationListRight.Count; i++)
                {
                    if (Time.fixedTime > (vibrationListRight[i].x + vibrationListRight[i].y))
                    {
                        deleteIndex.Add(vibrationListRight[i]);
                    }
                    else
                    {
                        if (vibrationListRight[i].z > maxStrength)
                            maxStrength = vibrationListRight[i].z;
                        Debug.Log("lejr");
                    }
                }

                foreach(Vector3 v in deleteIndex)
                {
                    vibrationListRight.Remove(v);
                }
                deleteIndex.Clear();
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
            vm.vibrationListRight.Add(new Vector3(Time.time, vibDuration, vibStrength));
        }

        public static void AddVibrateLeft(float vibStrength, float vibDuration)
        {
            vm.vibrationListLeft.Add(new Vector3(Time.time, vibDuration, vibStrength));
        }

        public static void StopVibRight()
        {
            vm.vibrationListRight.Clear();
        }

        public static void StopVibLeft()
        {
            vm.vibrationListLeft.Clear();
        }


        public static void StopVibration()
        {
            StopVibRight();
            StopVibLeft();
        }

        private static void controllerTester()
        {
            GamePad.SetVibration(vm.pIndex, -vm.swarm.cursor.position.x / 10, vm.swarm.cursor.position.x / 10);
        }

        private void OnDestroy()
        {
            GamePad.SetVibration(pIndex, 0, 0);
        }
    }
}

