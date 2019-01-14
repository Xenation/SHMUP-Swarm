using UnityEngine;

namespace Swarm {
	public class Lazer : TelegraphableAttack {
		
		private BoxCollider2D col;

		protected override void OnAwake() {
			col = attack.GetComponent<BoxCollider2D>();
			
		}

		public void SetWidth(float width) {
			Transform rayTransf = attack.transform.Find("Ray");
			rayTransf.localScale = new Vector3(rayTransf.localScale.x, width, rayTransf.localScale.z);
			Transform startTransf = attack.transform.Find("Start");
			startTransf.localScale = new Vector3(width, width, startTransf.localScale.z);
			col.size = new Vector2(col.size.x, width);
            AkSoundEngine.PostEvent("Play_Laser", gameObject);
        }

        private void OnDestroy() {
            AkSoundEngine.PostEvent("Stop_Laser", gameObject);
        }
    }
}
