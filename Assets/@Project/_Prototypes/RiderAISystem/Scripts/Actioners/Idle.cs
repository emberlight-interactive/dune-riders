using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.Actioners {
	public class Idle : Actioner
	{
		public override bool currentlyActive {get => true;}

		public override void StartAction() {}
		public override void EndAction() {}
	}
}
