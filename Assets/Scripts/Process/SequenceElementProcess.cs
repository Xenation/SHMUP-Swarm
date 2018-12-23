using System.Collections.Generic;
using UnityEngine;
using Xenon;
using Xenon.Processes;

namespace Swarm {
	public class SequenceElementProcess : CompositeProcess {

		public SequenceElementProcess(SequenceElement seqElem, Pattern.RuntimeParameters runParams) {
			switch (seqElem.type) {
				case SequenceElementType.Bullet:
					float count = seqElem.GetField("Count").intValue;
					for (int i = 0; i < count; i++) {
						AddProcess(new ShootProcess(seqElem.GetField("Projectile").projectileValue, runParams.attackPoints, seqElem.GetField("Speed Override").floatValue));
						AddProcess(new TimedProcess(seqElem.GetField("Duration").floatValue / count));
					}
					break;
				case SequenceElementType.Lazer:
					//AddProcess(new ShootProcess(seqElem.projectile, points));
					//AddProcess(new TimedProcess(seqElem.duration));
					break;
				case SequenceElementType.Mortar:
					//for (int i = 0; i < seqElem.count; i++) {
					//	AddProcess(new ShootProcess(seqElem.projectile, points));
					//	AddProcess(new TimedProcess(seqElem.duration / seqElem.count));
					//}
					break;
				case SequenceElementType.Delay:
					AddProcess(new TimedProcess(seqElem.GetField("Duration").floatValue));
					break;
				case SequenceElementType.EnablePoint:
					AddProcess(new AttackPointStateProcess(runParams.attackPoints, seqElem.GetField("Point index").intValue, true));
					break;
				case SequenceElementType.DisablePoint:
					AddProcess(new AttackPointStateProcess(runParams.attackPoints, seqElem.GetField("Point index").intValue, false));
					break;
				case SequenceElementType.SetRotationAbsolute:
					AddProcess(new SetRotationAbsProcess(runParams, seqElem.GetField("Absolute Rotation").floatValue));
					break;
				case SequenceElementType.SetRotationSpeed:
					AddProcess(new SetRotationSpeedProcess(runParams, seqElem.GetField("Rotation Speed").floatValue));
					break;
			}
		}

	}
}
