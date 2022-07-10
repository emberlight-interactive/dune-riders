using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.State;
using Pathfinding;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(Halt))]
	[RequireComponent(typeof(Gunner))]
	public class HaltAndAttack : ActionerParallelizer
	{
		Actioner[] _actionersToRun = new Actioner[2];
		public override Actioner[] actionersToRun {
			get => _actionersToRun;
		}

		void Awake() {
			_actionersToRun[0] = GetComponent<Halt>();
			_actionersToRun[1] = GetComponent<Gunner>();
		}
	}
}
