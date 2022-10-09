using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DuneRiders.HomeVillageSystem;
using TMPro;

namespace DuneRiders.PlayerRiderControllerCombination {
	[RequireComponent(typeof(Slider))]
	public class HomeVillageFuelSliderUpdater : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI amountAndCapacityText;
		[SerializeField] TextMeshProUGUI burnRateText;
		[SerializeField] string burnRateUnits = "L/h";

		HomeVillageFuelManager homeVillageFuelManager;
		Slider homeVillageFuelSlider;

		void Awake() {
			homeVillageFuelManager = FindObjectOfType<HomeVillageFuelManager>();
			homeVillageFuelSlider = GetComponent<Slider>();

			if (homeVillageFuelManager == null) enabled = false;
		}

		void OnEnable() {
			StartCoroutine(UpdateSlider());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		IEnumerator UpdateSlider() {
			while (true) {
				homeVillageFuelSlider.value = homeVillageFuelManager.GetPercentageOfVillageFuelLeft();
				amountAndCapacityText.text = ((int) homeVillageFuelManager.FuelResourceManager.Amount()).ToString();

				if (homeVillageFuelManager.GetCurrentFuelPerHourConsumption() > 0) burnRateText.text = GetBurnRateString(homeVillageFuelManager.GetCurrentFuelPerHourConsumption());
				else burnRateText.text = "";

				yield return new WaitForSeconds(1f);
			}
		}

		string GetBurnRateString(float burnRate) {
			return $"-{burnRate} {burnRateUnits}";
		}
	}
}
