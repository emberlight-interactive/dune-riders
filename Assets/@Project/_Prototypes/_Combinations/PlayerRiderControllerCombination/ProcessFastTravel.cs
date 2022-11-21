using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DuneRiders.RiderAI.State;
using DuneRiders.RiderAI.Actioners;
using DuneRiders.Combinations;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class ProcessFastTravel : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI warningText;
		[SerializeField] FastTravel fastTravel;
		[SerializeField] FastTravelFuelCost fastTravelFuelCost;
		[SerializeField] EnemyAIEntitiesInRange enemyAIEntitiesInRange;
		[SerializeField] GraphicRaycaster mapRaycaster;
		[SerializeField] OVRScreenFade riderFader;
		[SerializeField] OVRScreenFade cockpitFader;
		[SerializeField] Transform entityTravelling;
		[SerializeField] Transform travelPosition;


		void Awake() {
			fastTravel.fastTravelFinished.AddListener(FinishFastTravel);
			fastTravel.fastTravelFinished.AddListener(TriggerFriendlyRiderSpawnNearPlayer);
			fastTravel.fastTravelFinished.AddListener(ChangePlayerCommandToFollow);
		}

		public void Process() {
			if (!fastTravelFuelCost.EnoughFuelForFastTravel()) {
				ShowWarningText("Insufficent Fuel");
			} else if (enemyAIEntitiesInRange.EntityCount > 0) {
				ShowWarningText("Enemies Nearby");
			} else {
				if (!fastTravel.CurrentlyTeleporting) StartFastTravel();
			}
		}

		void StartFastTravel() {
			mapRaycaster.enabled = false;
			FadeOut(riderFader);
			FadeOut(cockpitFader);
			Invoke(nameof(FastTravel), 0.7f);
		}

		void FastTravel() {
			fastTravelFuelCost.ProcessPayment();
			fastTravel.entityToFastTravel = entityTravelling;
			fastTravel.positionToTravelTo = travelPosition;
			fastTravel.Teleport();
		}

		void FinishFastTravel() {
			mapRaycaster.enabled = true;

			var rb = entityTravelling.GetComponent<Rigidbody>();
			if (rb != null) rb.velocity = Vector3.zero;

			FadeIn(riderFader);
			FadeIn(cockpitFader);
		}

		void FadeOut(OVRScreenFade fader) {
			fader.fadeTime = 0.5f;
			fader.FadeOut();
		}

		void FadeIn(OVRScreenFade fader) {
			fader.fadeTime = 0.5f;
			fader.FadeIn();
		}

		void ShowWarningText(string warning) {
			warningText.text = warning;
			warningText.enabled = true;
			Invoke(nameof(HideWarningText), 1.5f);
		}

		void HideWarningText() {
			warningText.enabled = false;
		}

		// Another instance of parallel systems
		void TriggerFriendlyRiderSpawnNearPlayer() {
			var friendlyRiders= GlobalQuery.GetAllCompanyRiders();

			foreach(var rider in friendlyRiders) {
				rider.GetComponent<TeleportNearPlayer>().StartAction();
			}
		}

		void ChangePlayerCommandToFollow() {
			var playerCommandState = FindObjectOfType<PlayerCommandState.PlayerCommandGlobalState>();
			if (playerCommandState != null) playerCommandState.command = PlayerCommandState.Command.Follow;
		}
	}
}
