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

		void Update() {
			fuelText.text = $"{(int) gatherer.GetManager(Gatherer.SupportedResources.Fuel).Amount()} L";
			fuelBurnRateText.text = $"-{burnRateSystem.GetResourceBurnRate(BurnRateSystem.ResourceType.Fuel)} L/h";
		}
	}
}
