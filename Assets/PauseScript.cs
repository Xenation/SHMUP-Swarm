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
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetAxis("Vertical") > 0.1f)
            {
                EventSystem.current.SetSelectedGameObject(continueButton.gameObject, null);
            }
            if(Input.GetAxis("Vertical") < -0.1f)
            {
                EventSystem.current.SetSelectedGameObject(menuButton.gameObject, null);
            }
        }

        private void onClickContinue()
        {
            swarm.switchPause(false);
        }

        private void onClickMenu()
        {
            SceneSwitcher.SwitchScene("Menu");
        }
    }

}
