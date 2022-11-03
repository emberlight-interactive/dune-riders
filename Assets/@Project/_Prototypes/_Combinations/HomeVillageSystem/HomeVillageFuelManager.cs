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
		public UnityEvent migrationLevelsChanged = new UnityEvent();
		public ResourceManager FuelResourceManager { get => fuelResourceManager; }
		public int fuelPerHour = 400;
		public int numberOfMigrations = 1;

		[SerializeField] int baseMigrationLevel = 5000;
		[SerializeField] int migrationIncrements = 1000;
		int migrationLevel { get => baseMigrationLevel + (numberOfMigrations * migrationIncrements); }
		[SerializeField] int reserveLevel = 5000;
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
					migrationLevelsChanged.Invoke();
					return true;
				} else return false;
			}

			return false;
		}

		IEnumerator FuelConsumption() {
			while (true) {
				yield return new WaitForSeconds(10f); // todo: Could exit and reload rapidly to cheat?

				var fuelPerMinute = fuelPerHour / 360f;
				if (!fuelResourceManager.Take(fuelPerMinute)) {
					villageOutOfFuel.Invoke();
				}
			}
		}

		public float GetPercentageOfVillageFuelLeft() {
			return fuelResourceManager.Amount() / fuelResourceManager.ResourceLimit();
		}

		public float VillageFuelPercentageForMigration() {
			return migrationLevel / fuelResourceManager.ResourceLimit();
		}

		public float VillageFuelPercentageForSustenance() {
			return reserveLevel / fuelResourceManager.ResourceLimit();
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
