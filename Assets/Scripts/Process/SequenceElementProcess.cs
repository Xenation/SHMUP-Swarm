using System.Collections.Generic;
using UnityEngine;
using Xenon;
using Xenon.Processes;

namespace Swarm {
	public class SequenceElementProcess : CompositeProcess {

		public SequenceElementProcess(AttackSequenceModule.SequenceElement seqElem, List<AttackPoint> points) {
			ProjectileDefinition def;
			if (!PatternManager.I.GetProjectileDefinition(seqElem.projectileName, out def)) return;

			switch (seqElem.type) {
				case AttackSequenceModule.SequenceElementType.Bullet:
					for (int i = 0; i < seqElem.count; i++) {
						AddProcess(new ShootProcess(def, points));
						AddProcess(new TimedProcess(seqElem.duration / seqElem.count));
					}
					break;
				case AttackSequenceModule.SequenceElementType.Lazer:
					AddProcess(new ShootProcess(def, points));
					AddProcess(new TimedProcess(seqElem.duration));
					break;
				case AttackSequenceModule.SequenceElementType.Mortar:
					for (int i = 0; i < seqElem.count; i++) {
						AddProcess(new ShootProcess(def, points));
						AddProcess(new TimedProcess(seqElem.duration / seqElem.count));
					}
					break;
				case AttackSequenceModule.SequenceElementType.Delay:
					AddProcess(new TimedProcess(seqElem.duration));
					break;
				case AttackSequenceModule.SequenceElementType.EnablePoint:
					AddProcess(new AttackPointStateProcess(seqElem.point, true));
					break;
				case AttackSequenceModule.SequenceElementType.DisablePoint:
					AddProcess(new AttackPointStateProcess(seqElem.point, false));
					break;
			}
		}

	}
}
