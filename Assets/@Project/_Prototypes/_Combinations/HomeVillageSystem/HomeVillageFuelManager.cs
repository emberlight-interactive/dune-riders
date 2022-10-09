using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DuneRiders.GatheringSystem;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.HomeVillageSystem {
	public class HomeVillageFuelManager : MonoBehaviour, IPersistent
	{
		[Serializable]
		class HomeVillageFuelManagerSerializable {
			public int numberOfMigrations;
		}

		[SerializeField] ResourceManager fuelResourceManager;
		[SerializeField] UnityEvent villageOutOfFuel = new UnityEvent();
		public ResourceManager FuelResourceManager { get => fuelResourceManager; }
		public int fuelPerHour = 750;
		public int numberOfMigrations = 1;
		public int reserveLevel { get => 5000; }
		public int migrationLevel { get => 10000; }
		public bool DisablePersistence { get => false; }
		string persistenceKey = "HomeVillageFuelManager";

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
				yield return new WaitForSeconds(10f); // todo: Could exit and reload rapidly to cheat?

				var fuelPerMinute = GetCurrentFuelPerHourConsumption() / 360f;
				if (!fuelResourceManager.Take(fuelPerMinute)) {
					villageOutOfFuel.Invoke();
				}
			}
		}

		public int GetCurrentFuelPerHourConsumption() {
			return fuelPerHour * numberOfMigrations;
		}

		public float GetPercentageOfVillageFuelLeft() {
			return fuelResourceManager.Amount() / fuelResourceManager.ResourceLimit();
		}

		public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save(persistenceKey, new HomeVillageFuelManagerSerializable {
				numberOfMigrations = this.numberOfMigrations,
			});
		}

        public void Load(IPersistenceUtil persistUtil) {
			var loadedHomeVillageFuelManager = persistUtil.Load<HomeVillageFuelManagerSerializable>(persistenceKey);
			numberOfMigrations = loadedHomeVillageFuelManager.numberOfMigrations;
		}
	}
}
