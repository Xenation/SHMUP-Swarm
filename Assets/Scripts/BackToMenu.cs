using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swarm {
	public class BackToMenu : MonoBehaviour {
		// Start is called before the first frame update
		void Start() {

		}

		public void backMenu() {
			SceneSwitcher.SwitchScene("Menu");
		}
	}
}
