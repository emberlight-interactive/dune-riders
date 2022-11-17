using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DuneRiders.GatheringSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class FuelDisplayUpdater : MonoBehaviour
	{
		[SerializeField] BurnRateSystem burnRateSystem;
		[SerializeField] Gatherer gatherer;
		[SerializeField] TextMeshProUGUI fuelBurnRateText;
		TextMeshProUGUI fuelText;

		void Awake() {
			fuelText = GetComponent<TextMeshProUGUI>();
		}

		void Start() {
			InvokeRepeating(nameof(UpdateText), 0f, 1f);
		}

		void UpdateText() {
			var playerFuelManager = gatherer.GetManager(Gatherer.SupportedResources.Fuel);

			if (playerFuelManager.Amount() / playerFuelManager.ResourceLimit() < 0.07f) fuelText.color = Color.red;
			else fuelText.color = Color.white;

			fuelText.text = $"{(int) gatherer.GetManager(Gatherer.SupportedResources.Fuel).Amount()} L";
			fuelBurnRateText.text = $"-{burnRateSystem.GetResourceBurnRate(BurnRateSystem.ResourceType.Fuel)} L/h";
		}
	}
}
