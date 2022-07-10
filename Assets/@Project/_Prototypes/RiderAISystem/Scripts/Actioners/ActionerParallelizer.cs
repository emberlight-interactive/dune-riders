using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI.Actioners {
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
