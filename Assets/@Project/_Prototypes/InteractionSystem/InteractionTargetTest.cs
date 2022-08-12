using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.InteractionSystem.WaveInteraction;
using DuneRiders.InteractionSystem.ThumbsUpDownInteraction;
using DuneRiders.InteractionSystem.OptionSelectionInteraction;
using DuneRiders.InteractionSystem.RangeSelectionInteraction;
using TMPro;

namespace DuneRiders.InteractionSystem {
	public class InteractionTargetTest : InteractionTarget
	{
		[SerializeField] TextMeshProUGUI dialogueTextArea;

		protected override void StartInteraction() {
			Debug.Log("Interaction Initiated");
			InitiateCurrentResponseRequester();
		}

		protected override void EndInteraction() {
			ForceCancelCurrentResponseRequester();
			SetCurrentNodeToRoot();
		}

		void Start() {
			dialogueTextArea.text = "";
		}

		protected override Node BuildInteractionTree() {
			return new Node() {
				responseRequester = new WaveResponseRequester(HandleWave, () => {}),

				confirm = new Node {
					responseRequester = new ThumbsUpDownResponseRequester(HandleThumbsOutput, () => {}),

					confirm = new Node {
						responseRequester = new OptionSelectionResponseRequester(
							(option) => {
								if (option == "Move Village") MoveVillage();
								else Nothing();
							},
							() => {},
							new string[] { "Move Village", "Poggeta", "What y doing" }
						),

						confirm = new Node {
							responseRequester = new RangeSelectionResponseRequester(
								RecieveFuel,
								() => {},
								1302
							),
						}
					}
				},
			};
		}

		void HandleWave(bool waved) {
			Debug.Log("Waved");
			SetCurrentNodeToConfirmNode();
			dialogueTextArea.text = "Do you thumbs up gang?";
			InitiateCurrentResponseRequester();
		}

		void HandleThumbsOutput(bool thumbsUp) {
			Debug.Log("Thumbs up output: " + thumbsUp);

			if (thumbsUp) SetCurrentNodeToConfirmNode();
			SetCurrentNodeToCancelNode();

			dialogueTextArea.text = "What are we doing chief?";
			InitiateCurrentResponseRequester();
		}

		void MoveVillage() {
			dialogueTextArea.text = "Guess we movin the village, we need gas doe";
			SetCurrentNodeToConfirmNode();
			InitiateCurrentResponseRequester();
		}

		void Nothing() {}

		void RecieveFuel(int fuel) {
			dialogueTextArea.text = $"{fuel} Should be enough fuel";
		}
	}
}
