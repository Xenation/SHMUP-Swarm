using System.Collections.Generic;
using UnityEngine;

namespace Swarm {
	[RequireComponent(typeof(PolygonCollider2D))]
	public class Cone : TelegraphableAttack {
		
		private PolygonCollider2D col;
		private Mesh mesh;

		private List<int> indices = new List<int>();
		private List<Vector3> vertices = new List<Vector3>();
		private List<Vector2> uvs = new List<Vector2>();

		protected override void OnAwake() {
			col = GetComponent<PolygonCollider2D>();
			col.enabled = false;
			mesh = GetComponent<MeshFilter>().mesh;
			AkSoundEngine.PostEvent("Play_Cone", gameObject);
		}

		public void SetAngleRadius(float angle, float radius) {
			Vector2[] poly = Utilities.GenerateCone(new Vector2(0.1f, 0f), angle * Mathf.Deg2Rad, radius, 0.3f);
			col.SetPath(0, poly);
			Utilities.GenerateConeMesh(indices, vertices, uvs, poly);
			mesh.Clear();
			mesh.SetVertices(vertices);
			mesh.SetUVs(0, uvs);
			mesh.SetTriangles(indices, 0);
		}

		public override void LaunchAttack() {
			base.LaunchAttack();
			col.enabled = true;
		}

		private void OnDestroy() {
            AkSoundEngine.PostEvent("Stop_Cone", gameObject);
        }
		
	}
}
