using System.Collections.Generic;
using UnityEngine;

namespace Swarm {
	public static class Utilities {

		public static Rect SubHorizontalRect(this ref Rect rect, float cutDistFromLeft, float margin = 0f) {
			Rect nRect = new Rect(rect.x, rect.y, cutDistFromLeft, rect.height);
			rect.x += cutDistFromLeft + margin;
			return nRect;
		}

		public static Rect SubVerticalRect(this ref Rect rect, float cutDistFromTop, float margin = 0f) {
			Rect nRect = new Rect(rect.x, rect.y, rect.width, cutDistFromTop);
			rect.y += cutDistFromTop + margin;
			return nRect;
		}
		
		public static void GenerateConeMesh(List<int> indices, List<Vector3> vertices, List<Vector2> uvs, Vector2[] poly) {
			int curveVertCount = poly.Length - 1;
			int triCount = curveVertCount - 1;
			indices.Clear();
			vertices.Clear();
			uvs.Clear();
			for (int i = 0; i < triCount; i++) {
				indices.Add(0);
				indices.Add(i + 2);
				indices.Add(i + 1);
			}
			for (int i = 0; i < poly.Length; i++) {
				vertices.Add(poly[i]);
			}
			uvs.Add(new Vector2(0f, 0.5f));
			uvs.Add(new Vector2(1f, 0f));
			for (int i = 2; i < poly.Length - 1; i++) {
				uvs.Add(new Vector2(1f, 0.5f));
			}
			uvs.Add(new Vector2(1f, 1f));
		}

		/// <summary>
		/// Returns a list of 2D points forming a cone (directed to the right)
		/// </summary>
		/// <param name="center">the center</param>
		/// <param name="angle">the angle in radians</param>
		/// <param name="radius">the radius</param>
		/// <param name="minVertexDistance">the minimum distance between vertices that form the curve</param>
		/// <returns>A list of 2D points forming a cone polygon</returns>
		public static Vector2[] GenerateCone(Vector2 center, float angle, float radius, float minVertexDistance = .5f) {
			int curveVertCount = GetVertexCountInCone(angle, radius, minVertexDistance);
			Vector2[] vertices = new Vector2[curveVertCount + 1];
			vertices[0] = center;
			float angleDelta = angle / curveVertCount;
			float currentAngle = -angle / 2f;
			for (int i = 0; i < curveVertCount; i++) {
				vertices[i + 1] = center + new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * radius;
				currentAngle += angleDelta;
			}
			return vertices;
		}

		/// <summary>
		/// Returns the number of vertices required to form a circle with a given minimum distance between each vertices
		/// </summary>
		/// <param name="radius">the radius of the circle</param>
		/// <param name="minDistance">the minimum distance between each vertex</param>
		/// <returns>the number of vertices required to form a circle with a given minimal distance between each vertices</returns>
		public static int GetVertexCountInCircle(float radius, float minDistance) {
			return Mathf.CeilToInt(2 * Mathf.PI * radius / minDistance);
		}

		/// <summary>
		/// Returns the number of vertices required to form the curve of a cone with a given minimum distance between each vertices
		/// </summary>
		/// <param name="angle">the angle of the cone</param>
		/// <param name="radius">the radius of the cone</param>
		/// <param name="minDistance">the minimum distance between each vertex</param>
		/// <returns>the number of vertices required to form the curve of a cone with a given minimum distance between each vertices</returns>
		public static int GetVertexCountInCone(float angle, float radius, float minDistance) {
			return Mathf.CeilToInt(angle * radius / minDistance);
		}

	}
}
