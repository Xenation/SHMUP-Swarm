using System;

namespace Swarm {
	[Serializable]
	public struct Phase {

		public PatternDefinition startPattern;
		public PatternDefinition resetPattern;
		public int lifeThreshold;

	}
}
