using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Swarm {
	public class menuController : MonoBehaviour {
		private GameObject buttonPlay;

		private GameObject option;

		private GameObject menu;

		private GameObject credit;

		public Slider sliderMusic;

		public Slider sliderVolume;

		// Start is called before the first frame update
		void Start() {
			GameObject.Find("ButtonPlay").GetComponent<Button>().onClick.AddListener(onClickPlay);
			GameObject.Find("ButtonTutorial").GetComponent<Button>().onClick.AddListener(onClickTutorial);
			GameObject.Find("ButtonScore").GetComponent<Button>().onClick.AddListener(onClickScore);
			GameObject.Find("ButtonOption").GetComponent<Button>().onClick.AddListener(openOptions);
			GameObject.Find("ButtonExit").GetComponent<Button>().onClick.AddListener(onClickExit);
			GameObject.Find("ButtonCredit").GetComponent<Button>().onClick.AddListener(openCredit);
			GameObject.Find("ButtonRetourCredit").GetComponent<Button>().onClick.AddListener(returnMenuCredit);
			GameObject.Find("ButtonRetourOption").GetComponent<Button>().onClick.AddListener(returnMenuOption);
			option = GameObject.Find("Option");
			menu = GameObject.Find("Menu");
			credit = GameObject.Find("Credit");
			option.SetActive(false);
			credit.SetActive(false);
		}

		// Update is called once per frame
		void Update() {

		}
		public void OnMove() {
			AkSoundEngine.PostEvent("Play_UI_Move", gameObject);
		}

		void onClickPlay() {
			SceneSwitcher.SwitchScene("PlayScene");
			AkSoundEngine.PostEvent("Play_UI_Start", gameObject);
		}

		void onClickExit() {
			Application.Quit();
		}

		void onClickTutorial() {
			SceneSwitcher.SwitchScene("TutorialScene");
			AkSoundEngine.PostEvent("Play_UI_Valide", gameObject);
		}

		void onClickScore() {
			SceneSwitcher.SwitchScene("Leaderboard");
			AkSoundEngine.PostEvent("Play_UI_Valide", gameObject);
		}

		void openOptions() {
			menu.SetActive(false);
			GameObject myEventSystem = GameObject.Find("EventSystem");
			option.SetActive(true);
			myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("MusicSlider"));
			AkSoundEngine.PostEvent("Play_UI_Valide", gameObject);
		}

		void openCredit() {
			menu.SetActive(false);
			GameObject myEventSystem = GameObject.Find("EventSystem");
			credit.SetActive(true);
			myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("ButtonRetourCredit"));
			AkSoundEngine.PostEvent("Play_UI_Valide", gameObject);
		}

		void returnMenuOption() {
			menu.SetActive(true);
			GameObject myEventSystem = GameObject.Find("EventSystem");
			myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("ButtonPlay"));
			option.SetActive(false);
			AkSoundEngine.PostEvent("Play_UI_Back", gameObject);
		}

		void returnMenuCredit() {
			menu.SetActive(true);
			GameObject myEventSystem = GameObject.Find("EventSystem");
			myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("ButtonPlay"));
			credit.SetActive(false);
			AkSoundEngine.PostEvent("Play_UI_Back", gameObject);
		}

		public void ChangeMusic() {
			AkSoundEngine.SetRTPCValue("VolumeMusic", sliderMusic.value);
		}

		public void ChangeVolume() {
			AkSoundEngine.SetRTPCValue("VolumeSfx", sliderVolume.value);
		}
	}
}
