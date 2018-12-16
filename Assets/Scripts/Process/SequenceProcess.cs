using System.Collections.Generic;

namespace Swarm {
	public class SequenceProcess : CompositeProcess {

		public SequenceProcess(List<AttackSequenceModule.SequenceElement> sequence, List<AttackPoint> points) {
			foreach (AttackSequenceModule.SequenceElement elem in sequence) {
				AddProcess(new SequenceElementProcess(elem, points));
			}
		}

	}
}
