using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Swarm {
	public class winMenu : MonoBehaviour {
		public Text name;
		public Text score;

		// Start is called before the first frame update
		void Start() {
			GameObject.Find("returnToMenu").GetComponent<Button>().onClick.AddListener(onClickMenu);
			GameObject.Find("SubmitScore").GetComponent<Button>().onClick.AddListener(onClickSubmit);
			score.text = ScoreManager.textScore;
		}

		// Update is called once per frame
		void Update() {

		}

		void onClickMenu() {
            AkSoundEngine.PostEvent("Stop_Music", gameObject);
            SceneSwitcher.SwitchScene("Menu");
		}

		void onClickSubmit() {
            AkSoundEngine.PostEvent("Stop_Music", gameObject);
            ScoreManager.sendScore(name.text); //Add Name from submit text field
		}
	}
}
