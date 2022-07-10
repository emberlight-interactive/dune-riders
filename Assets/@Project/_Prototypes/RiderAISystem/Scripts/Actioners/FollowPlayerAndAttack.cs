using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(FollowPlayer))]
	[RequireComponent(typeof(Gunner))]
	public class FollowPlayerAndAttack : ActionerParallelizer
	{
		Actioner[] _actionersToRun = new Actioner[2];
		public override Actioner[] actionersToRun {
			get => _actionersToRun;
		}

		void Awake() {
			_actionersToRun[0] = GetComponent<FollowPlayer>();
			_actionersToRun[1] = GetComponent<Gunner>();
		}
	}
}
