using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DuneRiders.HomeVillageSystem;
using DuneRiders.RiderTabletSystem;
using TMPro;

namespace DuneRiders.PlayerRiderControllerCombination {
	[RequireComponent(typeof(Slider))]
	public class HomeVillageFuelSliderUpdater : MonoBehaviour
	{
		[SerializeField] ShowOnSliderValue showOnSliderValue;
		[SerializeField] RectTransform sliderFill;
		[SerializeField] RectTransform sustenanceMarker;
		[SerializeField] RectTransform migrationMarker;
		[SerializeField] TextMeshProUGUI amountAndCapacityText;
		[SerializeField] TextMeshProUGUI burnRateText;
		[SerializeField] string burnRateUnits = "L/h";

		HomeVillageFuelManager homeVillageFuelManager;
		Slider homeVillageFuelSlider;

		void Awake() {
			homeVillageFuelManager = FindObjectOfType<HomeVillageFuelManager>();
			homeVillageFuelSlider = GetComponent<Slider>();

			if (homeVillageFuelManager == null) enabled = false;

			homeVillageFuelManager.migrationLevelsChanged.AddListener(UpdateFuelLevelMarkers);
		}

		void OnEnable() {
			StartCoroutine(UpdateSlider());
			UpdateFuelLevelMarkers();
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		IEnumerator UpdateSlider() {
			while (true) {
				homeVillageFuelSlider.value = homeVillageFuelManager.GetPercentageOfVillageFuelLeft();
				amountAndCapacityText.text = ((int) homeVillageFuelManager.FuelResourceManager.Amount()).ToString();

				if (homeVillageFuelManager.fuelPerHour > 0) burnRateText.text = GetBurnRateString(homeVillageFuelManager.fuelPerHour);
				else burnRateText.text = "";

				yield return new WaitForSeconds(1f);
			}
		}

		void UpdateFuelLevelMarkers() {
			var sliderRect = homeVillageFuelSlider.GetComponent<RectTransform>().rect;
			var sliderWidth = sliderRect.width - 10;
			var sustenancePercentage = homeVillageFuelManager.VillageFuelPercentageForSustenance();
			var migrationPercentage = homeVillageFuelManager.VillageFuelPercentageForMigration();

			showOnSliderValue.valueAtOrAboveToShow = migrationPercentage;
			sustenanceMarker.anchoredPosition = new Vector2(sliderWidth * sustenancePercentage, sustenanceMarker.anchoredPosition.y);
			migrationMarker.anchoredPosition = new Vector2(sliderWidth * migrationPercentage, migrationMarker.anchoredPosition.y);
		}

		string GetBurnRateString(float burnRate) {
			return $"-{burnRate} {burnRateUnits}";
		}
	}
}
