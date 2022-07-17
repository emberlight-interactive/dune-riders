using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.AI {
	/// NOTE: Only use this if you want actioners to be completley restarted
	/// and run at the exact time as other actions to help with syncing timers etc
	public abstract class ActionerParallelizer : Actioner
	{
		public abstract Actioner[] actionersToRun {get;}
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		public override void StartAction()
		{
			if (!currentlyActive) {
				for (int i = 0; i < actionersToRun.Length; i++) {
					actionersToRun[i].StartAction();
				}
			}

			_currentlyActive = true;
		}

		public override void EndAction() {
			if (currentlyActive) {
				for (int i = 0; i < actionersToRun.Length; i++) {
					actionersToRun[i].EndAction();
				}

			}

			_currentlyActive = false;
		}
	}
}
