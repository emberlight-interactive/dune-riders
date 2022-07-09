using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI.Actioners {
	public class DespawnMySelf : Actioner
	{
		public override bool currentlyActive {get => false;}

		public override void StartAction()
		{
			gameObject.SetActive(false);
		}

		public override void EndAction() {}

	}
}
