using System.Collections.Generic;
using UnityEngine;
using Xenon;
using Xenon.Processes;

namespace Swarm {
	public class SequenceElementProcess : CompositeProcess {

		public SequenceElementProcess(SequenceElement seqElem, List<AttackPoint> points) {
			switch (seqElem.type) {
				case SequenceElementType.Bullet:
					for (int i = 0; i < seqElem.count; i++) {
						AddProcess(new ShootProcess(seqElem.GetField("Projectile").projectileValue, points));
						AddProcess(new TimedProcess(seqElem.GetField("Duration").floatValue / seqElem.GetField("Count").intValue));
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
					AddProcess(new AttackPointStateProcess(points, seqElem.GetField("Point index").intValue, true));
					break;
				case SequenceElementType.DisablePoint:
					AddProcess(new AttackPointStateProcess(points, seqElem.GetField("Point index").intValue, false));
					break;
			}
		}

	}
}
