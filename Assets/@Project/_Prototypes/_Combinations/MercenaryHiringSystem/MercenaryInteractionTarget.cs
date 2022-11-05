using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.State;
using DuneRiders.GatheringSystem;
using DuneRiders.Combinations;
using DuneRiders.InteractionSystem;
using DuneRiders.InteractionSystem.WaveInteraction;
using DuneRiders.InteractionSystem.ThumbsUpDownInteraction;
using TMPro;

namespace DuneRiders.MercenaryHiringSystem {
	public class MercenaryInteractionTarget : InteractionTarget
	{
		Gatherer gatherer;
		Mercenary mercenary;

		GameObject mercenaryPlaceholder;
		[SerializeField] Transform riderPlaceholderLocation;

		[SerializeField] Rider friendlyRiderPrefab;

		[SerializeField] AvailableMercenaryProvider availableMercenaryProvider;

		[SerializeField] GameObject interactionParent;

		[SerializeField] LookAt questionMark;
		[SerializeField] LookAt mercenaryOne;
		[SerializeField] LookAt mercenaryTwo;
		[SerializeField] LookAt promptParent;

		[SerializeField] Canvas promptCanvas;
		[SerializeField] TextMeshProUGUI prompt;

		public UnityEvent mercenaryHiredEvent = new UnityEvent();

		[Serializable]
		struct AvailableMercenaries {
			GameObject Mercenary;
			GameObject MercenaryDummy;
			int preciousMetalCost;
		}

		void Start() {
			gatherer = FindObjectOfType<Gatherer>();
			InitMercenaryInfoForThisLocation();
			Invoke("InitMercenaryPlaceholder", 0.5f);
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
				responseRequester = new WaveResponseRequester(this.StartMercenaryNegotiation, this.HandleCancel),

				confirm = new Node {
					responseRequester = new ThumbsUpDownResponseRequester(this.ResponseToOffer, this.HandleCancel),
				}
			};
		}

		void HandleCancel() {}

		void StartMercenaryNegotiation(bool waved) {
			SetInteractionSpotToActive();

			if (GlobalQuery.GetAllCompanyRiders().Length >= 10) {
				SetPromptText("Looks like you have a full company, come back if you need us");
				StartCoroutine(DelayedInteractionRestart());
			} else {
				SetPromptText($"Would you like us to join you for <color=yellow>{mercenary.preciousMetalCost}</color> precious metal?");
				SetCurrentNodeToConfirmNode();
				InitiateCurrentResponseRequester();
			}
		}

		void ResponseToOffer(bool thumbsUp) {
			if (thumbsUp) {
				if (gatherer.GetManager(Gatherer.SupportedResources.PreciousMetal).Take(mercenary.preciousMetalCost)) {
					AddMercenaryToParty();
				} else {
					SetPromptText("Sorry, it appears you do not have enough resources for payment");
					StartCoroutine(DelayedInteractionRestart());
				}
			} else {
				InteractionRestart();
			}
		}

		void AddMercenaryToParty() {
			mercenaryPlaceholder.SetActive(false);
			SpawnFriendlyRider();
			mercenaryHiredEvent.Invoke();
			interactionParent.SetActive(false);
		}

		void SetInteractionSpotToActive() {
			mercenaryOne.target = Camera.main.transform;
			mercenaryTwo.target = Camera.main.transform;
			questionMark.gameObject.gameObject.SetActive(false);
			promptParent.enabled = true;
			promptCanvas.enabled = true;
		}

		void SetInteractionSpotToIdle() {
			mercenaryOne.target = mercenaryTwo.transform;
			mercenaryTwo.target = mercenaryOne.transform;
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

		void InitMercenaryInfoForThisLocation() {
			mercenary = availableMercenaryProvider.GetMercenaryInformation();
		}

		void InitMercenaryPlaceholder() {
			var mercenaryPrefab = availableMercenaryProvider.MercenaryPrefab;

			mercenaryPrefab.chasisType = mercenary.chassis;
			mercenaryPrefab.gunType = mercenary.gunType;
			mercenaryPrefab.GetComponent<IsParkedState>().isParked = true;

			mercenaryPlaceholder = Instantiate(mercenaryPrefab.gameObject, riderPlaceholderLocation);
		}

		void SpawnFriendlyRider() {
			friendlyRiderPrefab.chasisType = mercenary.chassis;
			friendlyRiderPrefab.gunType = mercenary.gunType;

			var rider = Instantiate(friendlyRiderPrefab, riderPlaceholderLocation.position, riderPlaceholderLocation.rotation);
			BubbleGameObjectToActiveScene.BubbleUp(rider.gameObject);
		}

		[SerializeField] bool hireMercenary;

		void OnValidate() {
			if (hireMercenary) {
				AddMercenaryToParty();
				hireMercenary = false;
			}
		}
	}
}
