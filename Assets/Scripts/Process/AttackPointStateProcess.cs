using System.Collections.Generic;
using Xenon;

namespace Swarm {
	public class AttackPointStateProcess : Process {

		private List<AttackPoint> points;
		private int index;
		private bool nState;

		public AttackPointStateProcess(List<AttackPoint> pt, int i, bool nSt) {
			points = pt;
			index = i;
			nState = nSt;
		}

		public override void OnBegin() {
			
		}

		public override void Update(float dt) {
			points[index].shootingEnabled = nState;
			Terminate();
		}

		public override void OnTerminate() {
			
		}
	}
}
