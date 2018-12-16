using UnityEngine;

namespace Swarm {
	public class AttackPoint : MonoBehaviour {

		public bool shootingEnabled = true;

		public Vector2 position {
			get {
				return transform.position;
			}
			set {
				transform.position = value;
			}
		}

		public float rotation {
			get {
				return transform.rotation.eulerAngles.z;
			}
			set {
				transform.rotation = Quaternion.Euler(0f, 0f, value);
			}
		}

		private void OnDrawGizmosSelected() {
			Color tmpCol = Gizmos.color;
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + transform.right + transform.up * .05f);
			Gizmos.DrawLine(transform.position, transform.position + transform.right - transform.up * .05f);
			Gizmos.color = tmpCol;
		}

	}
}
