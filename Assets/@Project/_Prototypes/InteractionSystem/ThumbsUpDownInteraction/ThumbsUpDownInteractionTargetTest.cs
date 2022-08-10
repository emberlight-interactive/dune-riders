using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.InteractionSystem.ThumbsUpDownInteraction {
	public class ThumbsUpDownInteractionTargetTest : InteractionTarget
	{
		protected override void StartInteraction() {
			var thumbs = new ThumbsUpDownResponseRequester(
				(thumbsUp) => {
					Debug.Log("Was the thumbs up: " + thumbsUp);
				},
				() => { /* close dialogue */ }
			);

			thumbs.Initiate();
		}

		protected override void EndInteraction() {}

		void Start() {
			StartInteraction();
		}
	}
}
