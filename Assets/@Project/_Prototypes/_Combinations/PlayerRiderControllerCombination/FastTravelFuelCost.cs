using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.HomeVillageSystem;
using DuneRiders.GatheringSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class FastTravelFuelCost : MonoBehaviour
	{
		[SerializeField] float averageKmPerHour = 80f;
		[SerializeField] BurnRateSystem burnRateSystem;
		[SerializeField] HomeVillageFuelManager homeVillageFuelManager;
		[SerializeField] Transform fastTravelLocation;
		[SerializeField] ResourceManager playerFuelManager;
		[SerializeField] ResourceManager homeVillageFuelResourceManager;

		float GetTravelTimeEstimateInHours() {
			return Vector3.Distance(transform.position, fastTravelLocation.position) / (averageKmPerHour * 1000);
		}

		public float GetHomeVillageTravelFuelCost() {
			return homeVillageFuelManager.fuelPerHour * GetTravelTimeEstimateInHours();
		}

		public float GetPlayerTravelFuelCost() {
			var playerCompanyFuelPerHour = burnRateSystem.GetResourceBurnRate(BurnRateSystem.ResourceType.Fuel);
			return playerCompanyFuelPerHour * GetTravelTimeEstimateInHours();
		}

		public bool EnoughFuelAtHomeVillage() {
			return GetHomeVillageTravelFuelCost() < homeVillageFuelResourceManager.Amount();
		}

		public bool EnoughFuelInPlayerCompany() {
			return GetPlayerTravelFuelCost() < playerFuelManager.Amount();
		}

		public bool EnoughFuelForFastTravel() {
			return EnoughFuelAtHomeVillage() && EnoughFuelInPlayerCompany();
		}

		public void ProcessPayment() {
			if (!playerFuelManager.Take(GetPlayerTravelFuelCost())) playerFuelManager.Take(playerFuelManager.Amount());
			if (!homeVillageFuelResourceManager.Take(GetHomeVillageTravelFuelCost())) homeVillageFuelResourceManager.Take(homeVillageFuelResourceManager.Amount());
		}
	}
}
