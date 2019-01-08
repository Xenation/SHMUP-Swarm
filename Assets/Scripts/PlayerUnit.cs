using UnityEngine;
using Xenon;

namespace Swarm {
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerUnit : MonoBehaviour {

		private Rigidbody2D rb;
		private PlayerSwarm swarm;

		private Vector2 velocity;

        

        private void Awake() {
			rb = GetComponent<Rigidbody2D>();
			swarm = GetComponentInParent<PlayerSwarm>();
		}

		private void Update() {
			Vector2 toCursor = swarm.cursor.position - transform.position;
			float cursorDistance = toCursor.magnitude;
			velocity = toCursor.normalized;
			if (cursorDistance < swarm.cursorRadius) {
				velocity *= cursorDistance / swarm.cursorRadius;
			}
			velocity *= swarm.unitSpeed;
			DebugVelocity(velocity, Color.green);

			float sqrUnitRaduis = swarm.unitRadius * swarm.unitRadius;
			Vector2 displace = Vector2.zero;
			foreach (PlayerUnit unit in swarm.units) {
				if (unit == this) continue;
				Vector2 toOther = unit.transform.position - transform.position;
				if (toOther.sqrMagnitude < sqrUnitRaduis) {
					displace -= toOther.normalized * (1f - toOther.sqrMagnitude / sqrUnitRaduis);
				}
			}
			DebugVelocity(displace, Color.yellow);
			velocity += displace;

			DebugVelocity(velocity, Color.blue);
		}

		private void FixedUpdate() {
			rb.velocity = velocity;
			rb.rotation = Vector2.SignedAngle(Vector2.up, velocity.normalized);

            

        }

		public void Suicide() {
			ProjectileUnit projUnit = gameObject.AddComponent<ProjectileUnit>();
			projUnit.Init(swarm);
			Destroy(this);
		}

		private void DebugVelocity(Vector2 vel, Color col) {
			if (!swarm.debug) return;
			Debug.DrawLine(transform.position, transform.position + (Vector3) vel, col);
		}
        

        private void OnDestroy()
        {
            //METTRE SON DE LA MORT
            swarm.RemoveUnit(this);
        }

    }
}