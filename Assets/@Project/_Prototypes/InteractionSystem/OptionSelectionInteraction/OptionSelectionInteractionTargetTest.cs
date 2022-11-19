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
				new SelectableOption[] {
					new SelectableOption { optionName = "Migrate Village" },
					new SelectableOption { optionName = "Poggers" },
					new SelectableOption { optionName = "3rd option" },
				}
			);

			optionSelection.Initiate();
		}

		protected override Node BuildInteractionTree() { return new Node(); }

		protected override void EndInteraction() {}

		void Start() {
			StartInteraction();
		}
	}
}
