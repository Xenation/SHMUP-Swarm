using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Swarm
{
    public class cinematicScript : MonoBehaviour
    {
        public VideoPlayer vp;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (vp.isPrepared && !vp.isPlaying)
            {
                SceneSwitcher.SwitchScene("TutorialScene");
            }
        }
    }

}
