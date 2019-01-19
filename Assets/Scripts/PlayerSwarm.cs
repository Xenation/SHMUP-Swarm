using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Swarm
{
    public class PlayerSwarm : MonoBehaviour
    {

        public GameObject unitPrefab;
        public GameObject shrinkPrefab;
        public int unitsToCreate = 50;

        public float cursorShrinkSpeed = 2.5f;
        public float cursorShrinkRadius = 0.5f;
        public float unitShrinkSpeed = 8f;
        public float unitShrinkRadius = 0.5f;

        public float cursorNormalSpeed = 5f;
        public float cursorNormalRadius = 1f;
        public float unitNormalSpeed = 4f;
        public float unitNormalRadius = .1f;

        public float suicideSpeed = 10f;
        public float freezeTime = 1.0f;
        public Transform bossTransform;
        private int nbOfUnits;

        public bool debug = false;
        private bool inShrink = false;
        [HideInInspector] public int ShrinkUnits;

        [HideInInspector]
        public float cursorSpeed = 5f;
        public float cursorRadius = 1f;
        public float unitSpeed = 4f;
        public float unitRadius = .1f;

        [System.NonSerialized] public List<PlayerUnit> units = new List<PlayerUnit>();
        [System.NonSerialized] public Transform cursor;

        private Vector2 velocity;
        private Rigidbody2D cursorRB;

        private float rtpcValue = 5.0f;
        private float distanceToBoss;

        private bool inPause = false;
        private float defaultTimeScale;

        private SpriteRenderer cursorSprite;

        private GameObject shrinkUnit;
        private float sizeRatio;

        public Text Pause;

        public bool tutorial = false;

        public GameObject backgroundPause;
        public GameObject pauseMenu;

        private void Awake()
        {
            cursor = transform.Find("Cursor");
            cursorRB = cursor.GetComponent<Rigidbody2D>();

            unitsToCreate = ScoreManager.pyusAtLastPhase;

            for (int i = 0; i < unitsToCreate; i++)
            {
                float perim = Random.Range(0f, Mathf.PI);
                float dist = Random.Range(0f, cursorRadius);
                Instantiate(unitPrefab, cursor.position + new Vector3(Mathf.Cos(perim) * dist, Mathf.Sin(perim) * dist), Quaternion.identity, transform);
            }
            ScoreManager.totalPyus += unitsToCreate;

            defaultTimeScale = Time.fixedDeltaTime;
            cursorSprite = cursor.GetComponent<SpriteRenderer>();
            GetComponentsInChildren(units);

            cursorSpeed = cursorNormalSpeed;
            cursorRadius = cursorNormalRadius;
            unitSpeed = unitNormalSpeed;
            unitRadius = unitNormalRadius;
        }

        private void Update()
        {
            if (!inPause)
            {
#if UNITY_EDITOR
				// DEBUG
				if (Input.GetKey(KeyCode.Keypad0)) {
					GameObject tmp = Instantiate(unitPrefab, new Vector2(Random.Range(cursor.position.x - 0.1f, cursor.position.x + 0.1f), Random.Range(cursor.position.y - 0.1f, cursor.position.y + 0.1f)), Quaternion.identity, transform);
					AddUnit(tmp);
				}
#endif

				velocity.x = Input.GetAxisRaw("Horizontal");
                velocity.y = Input.GetAxisRaw("Vertical");
                //velocity.Normalize();

                if (Input.GetButtonDown("Fire1") && units.Count > 0 && cursorSpeed != cursorShrinkSpeed)
                {
                    if (units.Count > 1)
                    {
                        PlayerUnit unit = units[0];
                        units.RemoveAt(0);
                        unit.Suicide();
                        ScoreManager.nbPyuShot++;
                    }

                }

                if (Input.GetAxisRaw("Fire2") >= 0.8f && !inShrink)
                {
                    cursorSpeed = cursorShrinkSpeed;
                    cursorRadius = cursorShrinkRadius;
                    unitSpeed = unitShrinkSpeed;
                    //unitRadius = unitShrinkRadius;

                    //Second version of shrink
                    inShrink = true;
                    shrinkUnit = Instantiate(shrinkPrefab, cursor.position, Quaternion.identity, transform);
                    shrinkUnit.transform.localScale = new Vector3(shrinkUnit.transform.localScale.x / 2, shrinkUnit.transform.localScale.y / 2, shrinkUnit.transform.localScale.z);

                    sizeRatio = shrinkUnit.transform.localScale.x;

                }
                else if(Input.GetAxisRaw("Fire2") <= 0.8f && inShrink)
                {
                    cursorSpeed = cursorNormalSpeed;
                    cursorRadius = cursorNormalRadius;
                    unitSpeed = unitNormalSpeed;
                    unitRadius = unitNormalRadius;
                    Destroy(shrinkUnit);

                    //Respawn pyus and change sprite back to cursor
                    for (int i = 0; i < ShrinkUnits; i++)
                    {
                        float perim = Random.Range(0f, Mathf.PI);
                        float dist = Random.Range(0f, cursorRadius);
                        Instantiate(unitPrefab, cursor.position + new Vector3(Mathf.Cos(perim) * dist, Mathf.Sin(perim) * dist), Quaternion.identity, transform);
                    }
                    GetComponentsInChildren(units);
                    inShrink = false;
                    ShrinkUnits = 0;
                }

                if (inShrink)
                {
                    //Kill all pyus and change cursor to a bigger pyu.
                    Vector3 minDist = new Vector3(cursor.transform.position.x - cursorShrinkRadius, cursor.transform.position.y - cursorShrinkRadius, 0);
                    Vector3 maxDist = new Vector3(cursor.transform.position.x + cursorShrinkRadius, cursor.transform.position.y + cursorShrinkRadius, 0);

                    //V2

                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i].transform.position.x <= maxDist.x &&
                            units[i].transform.position.x >= minDist.x &&
                            units[i].transform.position.y <= maxDist.y &&
                            units[i].transform.position.y >= minDist.y)
                        {
                            if (units.Count > 1)
                            {
                                float sizeNow = (float)ShrinkUnits / ((float)ShrinkUnits + (float)units.Count);
                                shrinkUnit.transform.localScale = new Vector3(sizeRatio + (sizeRatio * sizeNow), sizeRatio + (sizeRatio * sizeNow), shrinkUnit.transform.localScale.z);
                            }
                            else if (units.Count == 1)
                            {
                                shrinkUnit.transform.localScale = new Vector3(sizeRatio + sizeRatio, sizeRatio + sizeRatio, shrinkUnit.transform.localScale.z);
                            }

                            Destroy(units[i].gameObject);

                            ++ShrinkUnits;
                        }
                    }
                }
            }
            if (inPause)
            {
                velocity.x = 0;
                velocity.y = 0;
            }


            /***********
             * Pause
             ***********/

            if (Input.GetButtonDown("Pause"))
            {
                if (inPause)
                {
                    switchPause(false);
                }
                else
                {
                    switchPause(true);
                }
            }

            velocity *= cursorSpeed;

            //Le nombre d'unitées est obtenable avec units.Count
            AkSoundEngine.SetRTPCValue("PyuNumber", (units.Count + ShrinkUnits));
            rtpcValue = Vector3.Distance(bossTransform.position, cursor.transform.position);
            AkSoundEngine.SetRTPCValue("ElectroFilter", rtpcValue);

            if (units.Count == 0 && (!inShrink || !shrinkUnit))
            {
                ScoreManager.endTime = Time.time;
                ScoreManager.bossDead = false;
                
                if (tutorial)
                {
                    SceneSwitcher.SwitchScene("TutorialScene");
                }
                else
                {
                    ScoreManager.nbOfLoses++;
                    AkSoundEngine.PostEvent("Stop_SFX", gameObject);
                    AkSoundEngine.PostEvent("Stop_Music", gameObject);
                    SceneSwitcher.SwitchScene("Lose");
                }

            }

        }

        public void switchPause(bool on)
        {
            if (on)
            {
                Time.timeScale = 0.001f;
                Time.fixedDeltaTime = defaultTimeScale * 0.001f;
                inPause = !inPause;
                Pause.text = "Pause \n Press P or Start to resume";
                backgroundPause.SetActive(true);
                pauseMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(GameObject.Find("Continue"));
            }
            else
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = defaultTimeScale;
                inPause = !inPause;
                Pause.text = "";
                backgroundPause.SetActive(false);
                pauseMenu.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            cursorRB.velocity = velocity;
        }

        //This function must sent vibrations to the controller according to the distance between the cursor and attacks / death
        //Adding a second collider to cursor might be helpfull for optimization
        private void ProximityToDanger()
        {
            //Check if gameObjects containing OnKillCollision are close
            //Add vibration of length fixedDuration
        }


        public void AddUnit(GameObject unit)
        {
            PlayerUnit testUnit = unit.GetComponent<PlayerUnit>();
            if (testUnit)
                units.Add(testUnit);
        }

        public void RemoveUnit(PlayerUnit unit)
        {
            units.Remove(unit);
        }

        public int getNbOfUnits()
        {
            if (inShrink)
                return units.Count + ShrinkUnits;
            else
                return units.Count;
        }
    }
}
