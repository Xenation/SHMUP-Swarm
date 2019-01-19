using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Xenon;
using Xenon.Processes;

namespace Swarm {
	public class SceneSwitcher : Singleton<SceneSwitcher> {

		private static bool created = false;

		private Graphic fader;

		private ProcessManager procManager;
		private string targetScene;
		private bool isSwitching = false;

		private void Awake() {
			if (created) {
				Destroy(gameObject);
				return;
			}
			created = true;
			procManager = new ProcessManager();
			fader = GetComponentInChildren<Graphic>();
			DontDestroyOnLoad(gameObject);

            //Disable mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

		}

		private void Update() {
			if (procManager == null) {
				Debug.LogWarning("[SceneSwitcher] - Update: procManager is null");
				return;
			}
			procManager.UpdateProcesses(Time.unscaledDeltaTime);
		}

		public static void SwitchScene(string sceneName, float fadeOutDuration = 1f, float fadeInDuration = 0.5f) {
			I.Switch(sceneName, fadeOutDuration, fadeInDuration);
		}

		public void Switch(string sceneName, float fadeOutDuration, float fadeInDuration) {
			if (isSwitching) return;
			isSwitching = true;
			targetScene = sceneName;
			FadeOutProcess fadeOut = new FadeOutProcess(fadeOutDuration, fader);
			fadeOut.TerminateCallback += FadeOutEnded;
			FadeInProcess fadeIn = new FadeInProcess(fadeInDuration, fader);
			fadeIn.TerminateCallback += FadeInEnded;
			fadeOut.Attach(fadeIn);
			procManager.LaunchProcess(fadeOut);
		}

		public void FadeOutEnded() {
			SceneManager.LoadScene(targetScene);
		}

		public void FadeInEnded() {
			isSwitching = false;
		}

	}
}
