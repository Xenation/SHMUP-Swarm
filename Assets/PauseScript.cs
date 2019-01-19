using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Swarm
{
    public class PauseScript : MonoBehaviour
    {
        // Start is called before the first frame update
        public PlayerSwarm swarm;

        public Button continueButton;
        public Button menuButton;
        private Button prevButton;
        private bool firstFrame = false;

        void Start()
        {
            continueButton.onClick.AddListener(onClickContinue);
            menuButton.onClick.AddListener(onClickMenu);

            EventSystem.current.SetSelectedGameObject(menuButton.gameObject, null);
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject, null);
        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(menuButton.gameObject, null);
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject, null);
            prevButton = continueButton;
            continueButton.Select();
            firstFrame = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetAxis("Vertical") > 0.1f)
            {
                EventSystem.current.SetSelectedGameObject(continueButton.gameObject, null);
                continueButton.Select();
                prevButton = continueButton;
            }
            else if(Input.GetAxis("Vertical") < -0.1f)
            {
                EventSystem.current.SetSelectedGameObject(menuButton.gameObject, null);
                menuButton.Select();
                prevButton = menuButton;
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(prevButton.gameObject, null);
                prevButton.Select();

                if(prevButton == continueButton && firstFrame)
                {
                    EventSystem.current.SetSelectedGameObject(menuButton.gameObject, null);
                    menuButton.Select();
                    EventSystem.current.SetSelectedGameObject(prevButton.gameObject, null);
                    prevButton.Select();
                    firstFrame = false;
                }
                else if(firstFrame)
                {
                    EventSystem.current.SetSelectedGameObject(continueButton.gameObject, null);
                    continueButton.Select();
                    EventSystem.current.SetSelectedGameObject(prevButton.gameObject, null);
                    prevButton.Select();
                    firstFrame = false;
                }
            }
        }

        private void onClickContinue()
        {
            swarm.switchPause(false);
        }

        private void onClickMenu()
        {
            swarm.switchPause(false);
            SceneSwitcher.SwitchScene("Menu");
        }
    }

}
