using System.Collections.Generic;
using Xenon.Processes;

namespace Swarm {
	public class SequenceProcess : CompositeProcess {

		public SequenceProcess(List<SequenceElement> sequence, List<AttackPoint> points, float endDelay) {
			foreach (SequenceElement elem in sequence) {
				AddProcess(new SequenceElementProcess(elem, points));
			}
			if (endDelay != 0f) {
				AddProcess(new TimedProcess(endDelay));
			}
		}

	}
}
