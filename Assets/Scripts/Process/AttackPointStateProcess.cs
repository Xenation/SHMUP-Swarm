using Xenon;

namespace Swarm {
	public class AttackPointStateProcess : Process {

		private AttackPoint point;
		private bool nState;

		public AttackPointStateProcess(AttackPoint pt, bool nSt) {
			point = pt;
			nState = nSt;
		}

		public override void OnBegin() {
			
		}

		public override void Update(float dt) {
			point.shootingEnabled = nState;
			Terminate();
		}

		public override void OnTerminate() {
			
		}
	}
}
