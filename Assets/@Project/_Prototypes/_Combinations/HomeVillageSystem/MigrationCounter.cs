using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DuneRiders.VillageMigrationSystem;

namespace DuneRiders.HomeVillageSystem {
	public class MigrationCounter : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI migrationCounter;
		VillageMigrationManager villageMigrator;

		void Awake() {
			villageMigrator = FindObjectOfType<VillageMigrationManager>();
		}

		void OnEnable() {
			migrationCounter.text = $"{GetNumberOfMigrationsLeft()} migration(s) left to reach Ring City";
		}

		int GetNumberOfMigrationsLeft() {
			int numberOfMigrations = 0;
			var currentWaypoint = villageMigrator.GetCurrentWaypoint();

			while(currentWaypoint != null){
				currentWaypoint = currentWaypoint.nextWaypoint;
				numberOfMigrations++;
			}

			return numberOfMigrations;
		}
	}
}
