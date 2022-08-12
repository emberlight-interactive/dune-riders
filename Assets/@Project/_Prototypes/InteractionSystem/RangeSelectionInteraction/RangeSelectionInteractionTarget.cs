using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.InteractionSystem.RangeSelectionInteraction {
	public class RangeSelectionInteractionTarget : InteractionTarget
	{
		protected override void StartInteraction() {
			var rangeSelector = new RangeSelectionResponseRequester(
				(value) => {
					Debug.Log("Value selected was : " + value);
				},
				() => { /* close dialogue */ },
				1200
			);

			rangeSelector.Initiate();
		}

		protected override Node BuildInteractionTree() { return new Node(); }

		protected override void EndInteraction() {}

		void Start() {
			StartInteraction();
		}
	}
}
