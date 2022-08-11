using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.InteractionSystem.WaveInteraction {
	public class WaveInteractionTargetTest : InteractionTarget
	{
		protected override void StartInteraction() {
			var wave = new WaveResponseRequester(
				(waved) => {
					Debug.Log("They waved: " + waved);
				},
				() => { /* close dialogue */ }
			);

			wave.Initiate();
		}

		protected override void EndInteraction() {}

		void Start() {
			StartInteraction();
		}
	}
}
