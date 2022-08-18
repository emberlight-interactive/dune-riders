using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GatheringSystem;

namespace DuneRiders.HomeVillageSystem {
	public class HomeVillageFuelManager : MonoBehaviour
	{
		[SerializeField] ResourceManager fuelResourceManager;
		public ResourceManager FuelResourceManager { get => fuelResourceManager; }
		public int fuelPerHour = 750;
		public int numberOfMigrations = 1;
		public int reserveLevel { get => 5000; }
		public int migrationLevel { get => 10000; }

		void OnEnable() {
			StartCoroutine(FuelConsumption());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		public bool CanMigrate() {
			return fuelResourceManager.Amount() >= migrationLevel;
		}

		public bool Migrate() {
			if (CanMigrate()) {
				if (fuelResourceManager.Take(migrationLevel - reserveLevel)) {
					numberOfMigrations++;
					return true;
				} else return false;
			}

			return false;
		}

		IEnumerator FuelConsumption() {
			while (true) {
				yield return new WaitForSeconds(60f);

				var fuelPerMinute = GetCurrentFuelPerHourConsumption() / 60;
				if (!fuelResourceManager.Take(fuelPerMinute)) {
					// todo: game over
				}
			}
		}

		public int GetCurrentFuelPerHourConsumption() {
			return fuelPerHour * numberOfMigrations;
		}
	}
}
