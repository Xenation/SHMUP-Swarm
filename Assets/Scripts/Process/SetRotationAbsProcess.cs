using UnityEngine;
using Xenon;

namespace Swarm {
	public class SetRotationAbsProcess : Process {

		private Pattern.RuntimeParameters runParams;
		private float rotation;

		public SetRotationAbsProcess(Pattern.RuntimeParameters runParams, float rot) {
			this.runParams = runParams;
			rotation = rot;
		}

		public override void OnBegin() {
			
		}

		public override void Update(float dt) {
			runParams.currentRotation = rotation;
			Terminate();
		}

		public override void OnTerminate() {
			
		}

	}
}
