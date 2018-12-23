using System.Collections.Generic;
using UnityEngine;
using Xenon.Processes;

namespace Swarm {
	public class SequenceProcess : CompositeProcess {

		public SequenceProcess(List<SequenceElement> sequence, Pattern.RuntimeParameters runParams, float endDelay) {
			foreach (SequenceElement elem in sequence) {
				AddProcess(new SequenceElementProcess(elem, runParams));
			}
			if (endDelay != 0f) {
				AddProcess(new TimedProcess(endDelay));
			}
		}

	}
}
