using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DuneRiders.GatheringSystem;
using DuneRiders.InteractionSystem;
using DuneRiders.InteractionSystem.WaveInteraction;
using DuneRiders.InteractionSystem.OptionSelectionInteraction;
using DuneRiders.InteractionSystem.RangeSelectionInteraction;
using TMPro;
using Sirenix.OdinInspector;

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

		[SerializeField] UnityEvent migrateVillage;

		bool currentlyPreparingForMigration = false;
		[ReadOnly] public bool nextMigrationTriggersWinCondition = false;

		Gatherer gatherer;
		string[] villageInteractionOptions = new string[] { "Transfer Fuel", "Migrate", "Ring City" };
		string[] availableVillageInteractionOptions = new string[3];

		void Start() {
			gatherer = FindObjectOfType<Gatherer>();
			SetInteractionSpotToIdle();
		}

		protected override void StartInteraction()
		{
			if (currentlyPreparingForMigration) {
				SetPromptText("We're getting the village prepared, we'll meet you there");
				SetInteractionSpotToActive();
			} else InitiateCurrentResponseRequester();
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
					responseRequester = new OptionSelectionResponseRequester(OptionSelected, HandleCancel, availableVillageInteractionOptions),
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
			CompileAvailableVillageOptions();
			SetCurrentNodeToConfirmNode();
			InitiateCurrentResponseRequester();
		}

		void CompileAvailableVillageOptions() {
			if (nextMigrationTriggersWinCondition) {
				availableVillageInteractionOptions[0] = villageInteractionOptions[0];
				availableVillageInteractionOptions[1] = villageInteractionOptions[2];
			} else {
				availableVillageInteractionOptions[0] = villageInteractionOptions[0];
				availableVillageInteractionOptions[1] = villageInteractionOptions[1];
			}
		}

		void OptionSelected(string option) {
			if (option == villageInteractionOptions[0]) {
				SetPromptText("How much Fuel would you like to give?");
				SetCurrentNodeToCustom(FuelTransferInteractionTree());
				InitiateCurrentResponseRequester();
			} else if (option == villageInteractionOptions[1] || option == villageInteractionOptions[2]) {
				AttemptMigration();
			} else InteractionRestart();
		}

		void AttemptMigration() {
			if (homeVillageFuelManager.Migrate()) {
				if (nextMigrationTriggersWinCondition) SetPromptText("");
				else SetPromptText("Great news! We'll get the village prepared and meet you there");
				currentlyPreparingForMigration = true;
				migrateVillage.Invoke();
			} else {
				SetPromptText("Unfortunately it looks like we don't have enough fuel");
				StartCoroutine(DelayedInteractionRestart());
			}
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

		public void MigrationFinished() {
			currentlyPreparingForMigration = false;
		}
	}
}
