using Xenon;

namespace Swarm {
	public class CompositeProcess : Process {

		private ProcessManager manager;
		private Process first;
		private Process last;

		public CompositeProcess() {
			manager = new ProcessManager();
		}

		public void AddProcess(Process process) {
			if (first == null) {
				first = last = process;
			} else {
				last.Attach(process);
				last = process;
			}
		}

		private void LastTerminated() {
			Terminate();
		}

		public override void OnBegin() {
			last.TerminateCallback += LastTerminated;
			manager.LaunchProcess(first);
		}

		public override void Update(float dt) {
			manager.UpdateProcesses(dt);
		}

		public override void OnTerminate() {
			
		}

	}
}
