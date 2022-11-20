using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DuneRiders.HomeVillageSystem {
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class UpdateFuelForMigrationText : MonoBehaviour
	{
		HomeVillageFuelManager homeVillageFuelManager;
		TextMeshProUGUI displayText;

		void Awake() {
			displayText = GetComponent<TextMeshProUGUI>();
			homeVillageFuelManager = FindObjectOfType<HomeVillageFuelManager>();
		}

		void OnEnable() {
			displayText.text = $"Village requires <color=#000>{(int) homeVillageFuelManager.FuelNeededForAnotherMigration()}</color> more fuel to migrate";
		}
	}
}
