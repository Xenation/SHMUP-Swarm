using System.Collections.Generic;
using UnityEngine;
using Xenon;
using Xenon.Processes;

namespace Swarm {
	public class SequenceElementProcess : CompositeProcess {

		public SequenceElementProcess(SequenceElement seqElem, Pattern.RuntimeParameters runParams) {
			switch (seqElem.type) {
				case SequenceElementType.Bullet:
					float count = seqElem.GetInt("Count");
					for (int i = 0; i < count; i++) {
						AddProcess(new ShootProcess(seqElem.GetProjectile("Projectile"), runParams.attackPoints, seqElem.GetFloat("Speed Override")));
						AddProcess(new TimedProcess(seqElem.GetFloat("Duration") / count));
					}
					break;
				case SequenceElementType.Lazer:
					AddProcess(new LazerProcess(runParams, seqElem.GetFloat("Width"), seqElem.GetFloat("Telegraph Duration"), seqElem.GetFloat("Duration"), seqElem.GetGameObject("Prefab").GetComponent<Lazer>()));
					break;
				case SequenceElementType.Mortar:
					AddProcess(new MortarProcess(runParams, seqElem.GetFloat("Aim Time"), seqElem.GetFloat("Radius"), seqElem.GetFloat("Seek Speed"), seqElem.GetFloat("Lock Time"), seqElem.GetGameObject("Prefab")));
					break;
				case SequenceElementType.Delay:
					AddProcess(new TimedProcess(seqElem.GetFloat("Duration")));
					break;
				case SequenceElementType.EnablePoint:
					AddProcess(new AttackPointStateProcess(runParams.attackPoints, seqElem.GetInt("Point index"), true));
					break;
				case SequenceElementType.DisablePoint:
					AddProcess(new AttackPointStateProcess(runParams.attackPoints, seqElem.GetInt("Point index"), false));
					break;
				case SequenceElementType.SetRotationAbsolute:
					AddProcess(new SetRotationAbsProcess(runParams, seqElem.GetFloat("Absolute Rotation")));
					break;
				case SequenceElementType.SetRotationSpeed:
					AddProcess(new SetRotationSpeedProcess(runParams, seqElem.GetFloat("Rotation Speed")));
					break;
				case SequenceElementType.Cone:
					AddProcess(new ConeProcess(runParams, seqElem.GetFloat("Angle"), seqElem.GetFloat("Range"), seqElem.GetFloat("Telegraph Duration"), seqElem.GetFloat("Duration"), seqElem.GetGameObject("Prefab").GetComponent<Cone>(), seqElem.GetGameObject("Telegraph Prefab")));
					break;
			}
		}

	}
}
