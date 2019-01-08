using UnityEngine;

namespace Swarm {
	[RequireComponent(typeof(BoxCollider2D))]
	public class Lazer : MonoBehaviour {

		private BoxCollider2D col;

		private void Awake() {
			col = GetComponent<BoxCollider2D>();
		}

		public void SetWidth(float width) {
			Transform rayTransf = transform.Find("Ray");
			rayTransf.localScale = new Vector3(rayTransf.localScale.x, width, rayTransf.localScale.z);
			Transform startTransf = transform.Find("Start");
			startTransf.localScale = new Vector3(width, width, startTransf.localScale.z);
			col.size = new Vector2(col.size.x, width);
		}

	}
}
