using UnityEngine;

namespace Swarm {
	public class SlideInCanon : MonoBehaviour {

		private Vector3 startPos = Vector3.left * .6f;
		private Vector3 targetPos = Vector3.right * 0.01f;

		private bool isSliding = false;
		private float slideDuration;
		private float slideStart;

		private void Awake() {
			transform.localPosition = startPos;
		}

		private void Update() {
			if (isSliding) {
				float progress = (Time.time - slideStart) / slideDuration;
				transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
				if (progress >= 1f) {
					isSliding = false;
					transform.localPosition = targetPos;
				}
			}
		}

		public void SlideIn(float duration) {
			isSliding = true;
			slideStart = Time.time;
			slideDuration = duration;
		}

	}
}
