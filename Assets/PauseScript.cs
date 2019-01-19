using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Swarm
{
    public class PauseScript : MonoBehaviour
    {
        // Start is called before the first frame update
        public PlayerSwarm swarm;

        void Start()
        {
            GameObject.Find("Continue").GetComponent<Button>().onClick.AddListener(onClickContinue);
            GameObject.Find("Menu").GetComponent<Button>().onClick.AddListener(onClickMenu);
        }

        // Update is called once per frame
        void Update()
        {

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
