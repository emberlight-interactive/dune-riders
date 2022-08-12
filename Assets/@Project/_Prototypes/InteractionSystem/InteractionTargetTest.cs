using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.InteractionSystem {
	public class InteractionTargetTest : InteractionTarget
	{
		protected override void StartInteraction() {
			Debug.Log("Interaction Initiated");
		}

		protected override void EndInteraction() {}
	}
}
