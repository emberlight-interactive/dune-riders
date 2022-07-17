using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;

namespace DuneRiders.OutpostAI.Actioners {
	public class Idle : Actioner
	{
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		public override void StartAction()
		{
			_currentlyActive = true;
		}

		public override void EndAction() {
			_currentlyActive = false;
		}
	}
}
