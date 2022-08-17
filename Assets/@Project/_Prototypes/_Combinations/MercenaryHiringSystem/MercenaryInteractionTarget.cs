using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

		// todo: Add option for setting which mercenary can spawn and how that's decided (it might be decided by an external script which filles details on instantiation)

		public GameObject mercenaryToHire;
		public int mercenaryCost = 75;

		[SerializeField] GameObject interactionParent;

		[SerializeField] LookAt questionMark;
		[SerializeField] LookAt mercenaryOne;
		[SerializeField] LookAt mercenaryTwo;
		[SerializeField] LookAt promptParent;

		[SerializeField] Canvas promptCanvas;
		[SerializeField] TextMeshProUGUI prompt;

		[SerializeField] GameObject dummyRider;

		[Serializable]
		struct AvailableMercenaries {
			GameObject Mercenary;
			GameObject MercenaryDummy;
			int preciousMetalCost;
		}

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
				responseRequester = new WaveResponseRequester(StartMercenaryNegotiation, HandleCancel),

				confirm = new Node {
					responseRequester = new ThumbsUpDownResponseRequester(ResponseToOffer, HandleCancel),
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
				SetPromptText($"Would you like us to join you for <color=yellow>{mercenaryCost}</color> precious metal?");
				SetCurrentNodeToConfirmNode();
				InitiateCurrentResponseRequester();
			}
		}

		void ResponseToOffer(bool thumbsUp) {
			if (thumbsUp) {
				if (gatherer.GetManager(Gatherer.SupportedResources.PreciousMetal).Take(mercenaryCost)) {
					dummyRider.SetActive(false);
					SimplePool.Spawn(mercenaryToHire, dummyRider.transform.position, dummyRider.transform.rotation); // todo: We need to centralize the spawning and despawning of riders or opt for destroying and instantiating
					interactionParent.SetActive(false);
				} else {
					SetPromptText("Sorry, it appears you do not have enough resources for payment");
					StartCoroutine(DelayedInteractionRestart());
				}
			} else {
				InteractionRestart();
			}
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
	}
}
