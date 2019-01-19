using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Swarm
{
    public class cinematicScript : MonoBehaviour
    {
        public VideoPlayer vp;
        bool playMusicOnce = true;
        // Start is called before the first frame update
        void Start()
        {
            AkSoundEngine.PostEvent("Play_Cinematic", gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (vp.isPrepared && !vp.isPlaying)
            {
                if (playMusicOnce)
                {
                    AkSoundEngine.PostEvent("Play_Music", gameObject);
                    playMusicOnce = false;
                }
                SceneSwitcher.SwitchScene("TutorialScene");
            }
        }
    }

}
