using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Swarm {
	public class winMenu : MonoBehaviour {
		public Text name;
		public Text score;

		private EventSystem eventSystem;
		private Button retMenu;
		private Button submit;
		private InputField nameField;
		private Selectable outOfField; // Ugly bypass to avoid the double down input

		private bool isSelectingDown = false;

		// Start is called before the first frame update
		void Start() {
			eventSystem = FindObjectOfType<EventSystem>();
			retMenu = GameObject.Find("returnToMenu").GetComponent<Button>();
			retMenu.onClick.AddListener(onClickMenu);
			submit = GameObject.Find("SubmitScore").GetComponent<Button>();
			submit.onClick.AddListener(onClickSubmit);
			nameField = GameObject.Find("NameField").GetComponent<InputField>();
			nameField.onEndEdit.AddListener(OnNameEndEdit);
			outOfField = GameObject.Find("OutOfField").GetComponent<Selectable>();
			score.text = ScoreManager.textScore;
            ScoreManager.nbOfWins++;
		}

		// Update is called once per frame
		void Update() {
			if (eventSystem.currentSelectedGameObject == nameField.gameObject) {
				if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("JoyVertical") < -0.1f) {
					Debug.Log("switching to submit");
					eventSystem.SetSelectedGameObject(outOfField.gameObject);
					isSelectingDown = true;
				}
			}
            else if(eventSystem.currentSelectedGameObject == submit.gameObject)
            {
                if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("JoyVertical") > 0.1f)
                {
                    //eventSystem.SetSelectedGameObject(nameField.gameObject);
                }
            }
		}

		void onClickMenu() {
            AkSoundEngine.PostEvent("Stop_Music", gameObject);
            SceneSwitcher.SwitchScene("Menu");
		}

		void onClickSubmit() {
            AkSoundEngine.PostEvent("Stop_Music", gameObject);
            ScoreManager.sendScore(name.text); //Add Name from submit text field
            SceneSwitcher.SwitchScene("Menu");
		}

		private void OnNameEndEdit(string name) {
			//onClickSubmit();
		}

	}
}
