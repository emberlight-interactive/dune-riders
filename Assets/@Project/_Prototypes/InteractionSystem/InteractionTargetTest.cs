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
		class Node {
			public ResponseRequesterBase responseRequester;
			public Node confirm;
			public Node cancel;
		}

		Node interactionTreeRoot;
		Node currentNode;

		[SerializeField] TextMeshProUGUI dialogueTextArea;

		protected override void StartInteraction() {
			Debug.Log("Interaction Initiated");
			InitiateCurrentResponseRequester();
		}

		protected override void EndInteraction() {}

		void Awake() {
			dialogueTextArea.text = "";
			BuildInteractionTree();
			SetCurrentNodeToRoot();
		}

		void BuildInteractionTree() {
			interactionTreeRoot =  new Node() {
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

		void SetCurrentNodeToRoot() {
			currentNode = interactionTreeRoot;
		}

		void InitiateCurrentResponseRequester() {
			currentNode.responseRequester.Initiate();
		}

		bool SetCurrentNodeToCancelNode() {
			if (currentNode.cancel == null) return false;

			currentNode = currentNode.cancel;
			return true;
		}

		bool SetCurrentNodeToConfirmNode() {
			if (currentNode.confirm == null) return false;

			currentNode = currentNode.confirm;
			return true;
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
