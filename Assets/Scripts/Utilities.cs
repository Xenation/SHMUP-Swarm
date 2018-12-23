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

	}
}
