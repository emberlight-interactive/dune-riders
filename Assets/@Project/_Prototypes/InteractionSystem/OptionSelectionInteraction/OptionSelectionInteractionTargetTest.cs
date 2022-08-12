using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.InteractionSystem.OptionSelectionInteraction {
	public class OptionSelectionInteractionTargetTest : InteractionTarget
	{
		protected override void StartInteraction() {
			var optionSelection = new OptionSelectionResponseRequester(
				(optionSelected) => {
					Debug.Log("They selected: " + optionSelected);
				},
				() => { /* close dialogue */ },
				new string[] { "Migrate Village", "Poggers", "3rd option" }
			);

			optionSelection.Initiate();
		}

		protected override void EndInteraction() {}

		void Start() {
			StartInteraction();
		}
	}
}
