using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GatheringSystem;

namespace DuneRiders.BonusGasSystem {
	public class ApplyGasBonus : MonoBehaviour
	{
		[SerializeField] ResourceManager playerFuelManager;
		[SerializeField] ResourceManager villageFuelManager;
		GameManager gameManager;

		void Awake() {
			gameManager = FindObjectOfType<GameManager>();
		}

		void Start() {
			if (gameManager.applyGasBonusOnMainSceneLoad) {
				ApplyGasBonusToResourceManagers();
				IndicateGasBonusHasBeenApplied();
			}
		}

		void ApplyGasBonusToResourceManagers() {
			var resourceManagers = new ResourceManager[] { playerFuelManager, villageFuelManager };
			foreach (var resourceManager in resourceManagers) {
				var thirdOfFuel = resourceManager.ResourceLimit() / 3;
				if (resourceManager.Amount() < thirdOfFuel) resourceManager.Give(thirdOfFuel);
			}
		}

		void IndicateGasBonusHasBeenApplied() {
			gameManager.applyGasBonusOnMainSceneLoad = false;
			gameManager.gasBonusesUsed++;
		}
	}
}
