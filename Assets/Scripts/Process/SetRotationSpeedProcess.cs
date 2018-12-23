using UnityEngine;
using Xenon;

namespace Swarm {
	public class SetRotationSpeedProcess : Process {

		Pattern.RuntimeParameters runParams;
		private float nSpeed;

		public SetRotationSpeedProcess(Pattern.RuntimeParameters runParams, float nSpeed) {
			this.runParams = runParams;
			this.nSpeed = nSpeed;
		}

		public override void OnBegin() {
			
		}

		public override void Update(float dt) {
			runParams.rotationSpeed = nSpeed;
			Terminate();
		}

		public override void OnTerminate() {

		}

	}
}
