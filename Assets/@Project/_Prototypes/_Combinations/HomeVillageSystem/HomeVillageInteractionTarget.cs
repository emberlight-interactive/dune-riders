using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GatheringSystem;
using DuneRiders.InteractionSystem;
using DuneRiders.InteractionSystem.WaveInteraction;
using DuneRiders.InteractionSystem.OptionSelectionInteraction;
using DuneRiders.InteractionSystem.RangeSelectionInteraction;
using TMPro;

namespace DuneRiders.HomeVillageSystem {
	public class HomeVillageInteractionTarget : InteractionTarget
	{
		[SerializeField] HomeVillageFuelManager homeVillageFuelManager;

		[SerializeField] LookAt questionMark;
		[SerializeField] LookAt guardOne;
		[SerializeField] LookAt guardTwo;
		[SerializeField] LookAt promptParent;

		[SerializeField] Canvas promptCanvas;
		[SerializeField] TextMeshProUGUI prompt;

		Gatherer gatherer;
		string[] villageInteractionOptions = new string[] { "Transfer Fuel" };

		void Start() {
			gatherer = FindObjectOfType<Gatherer>();
			SetInteractionSpotToIdle();
		}

		protected override void StartInteraction()
		{
			InitiateCurrentResponseRequester();
		}

		protected override void EndInteraction()
		{
			StopAllCoroutines();
			ForceCancelCurrentResponseRequester();
			InteractionReset();
		}

		protected override Node BuildInteractionTree()
		{
			return new Node {
				responseRequester = new WaveResponseRequester(StartVillageOptions, HandleCancel),

				confirm = new Node {
					responseRequester = new OptionSelectionResponseRequester(OptionSelected, HandleCancel, villageInteractionOptions),
				}
			};
		}

		Node FuelTransferInteractionTree() {
			return new Node {
				responseRequester = new RangeSelectionResponseRequester(FuelToTransfer, HandleCancel, gatherer.GetManager(Gatherer.SupportedResources.Fuel).Amount()),
			};
		}

		void HandleCancel() {
			InteractionRestart();
		}

		void StartVillageOptions(bool waved) {
			SetInteractionSpotToActive();
			SetPromptText("Hello, what would you like to do?");
			SetCurrentNodeToConfirmNode();
			InitiateCurrentResponseRequester();
		}

		void OptionSelected(string option) {
			if (option == villageInteractionOptions[0]) {
				SetPromptText("How much Fuel would you like to give?");
				SetCurrentNodeToCustom(FuelTransferInteractionTree());
				InitiateCurrentResponseRequester();
			} else InteractionRestart();
		}

		void FuelToTransfer(int value) {
			if (gatherer.GetManager(Gatherer.SupportedResources.Fuel).Take(value)) {
				if (homeVillageFuelManager.FuelResourceManager.Give(value)) {
					SetPromptText("Thank you for your help rider");
				} else SetPromptText("It looks like we have no space for that");
			} else SetPromptText("It looks like you no longer have that amount of fuel");

			StartCoroutine(DelayedInteractionRestart());
		}

		void SetInteractionSpotToActive() {
			guardOne.target = Camera.main.transform;
			guardTwo.target = Camera.main.transform;
			questionMark.gameObject.gameObject.SetActive(false);
			promptParent.enabled = true;
			promptCanvas.enabled = true;
		}

		void SetInteractionSpotToIdle() {
			guardOne.target = guardTwo.transform;
			guardTwo.target = guardOne.transform;
			questionMark.gameObject.gameObject.SetActive(true);
			promptParent.enabled = false;
			promptCanvas.enabled = false;
		}

		void SetPromptText(string promptText) {
			if (promptText == "") {
				promptCanvas.enabled = false;
			} else {
				promptCanvas.enabled = true;
			}

			prompt.text = promptText;
		}

		IEnumerator DelayedInteractionRestart(float time = 5f) {
			yield return new WaitForSeconds(time);
			InteractionRestart();
		}

		void InteractionRestart() {
			InteractionReset();
			InitiateCurrentResponseRequester();
		}

		void InteractionReset() {
			SetPromptText("");
			SetInteractionSpotToIdle();
			SetCurrentNodeToRoot();
		}
	}
}
