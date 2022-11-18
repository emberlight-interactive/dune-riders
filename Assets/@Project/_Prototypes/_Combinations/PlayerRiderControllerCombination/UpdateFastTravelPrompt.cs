using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class UpdateFastTravelPrompt : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI playerFuelCost;
		[SerializeField] TextMeshProUGUI homeFuelCost;
		[SerializeField] FastTravelFuelCost fastTravelFuelCost;

		void OnEnable() {
			UpdatePrompt();
		}

		public void UpdatePrompt() {
			playerFuelCost.text = $"- {Mathf.CeilToInt(fastTravelFuelCost.GetPlayerTravelFuelCost())}";
			playerFuelCost.color = fastTravelFuelCost.EnoughFuelInPlayerCompany() ? Color.white : Color.red;

			homeFuelCost.text = $"- {Mathf.CeilToInt(fastTravelFuelCost.GetHomeVillageTravelFuelCost())}";
			homeFuelCost.color = fastTravelFuelCost.EnoughFuelAtHomeVillage() ? Color.white : Color.red;
		}
	}
}
