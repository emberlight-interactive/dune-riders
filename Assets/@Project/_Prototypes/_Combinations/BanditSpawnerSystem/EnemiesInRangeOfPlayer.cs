using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.State;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.BanditSpawnerSystem {
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(AllActiveTurretsState))]
	public class EnemiesInRangeOfPlayer : MonoBehaviour
	{
		[SerializeField] Transform player;
		[SerializeField] Allegiance enemyAllegiance = Allegiance.Bandits;
		[SerializeField] float rangeDistance = 600f;

		AllActiveRidersState allActiveRidersState;
		AllActiveTurretsState allActiveTurretsState;

		void Awake() {
			if (player == null) player = FindObjectOfType<Player>().transform;
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			allActiveTurretsState = GetComponent<AllActiveTurretsState>();
		}

		public bool AreEnemyTurretsInRangeOfPlayer() {
			var allEnemyTurrets = allActiveTurretsState.GetAllTurretsOfAllegiance(enemyAllegiance);

			foreach (var turret in allEnemyTurrets) {
				if (Vector3.Distance(player.position, turret.transform.position) <= rangeDistance) {
					return true;
				}
			}

			return false;
		}

		public bool AreEnemyRidersInRangeOfPlayer() {
			var allEnemyRiders = allActiveRidersState.GetAllRidersOfAllegiance(enemyAllegiance);

			foreach (var rider in allEnemyRiders) {
				if (Vector3.Distance(player.position, rider.transform.position) <= rangeDistance) {
					return true;
				}
			}

			return false;
		}

		public bool AreEnemiesInRangeOfPlayer() {
			return AreEnemyTurretsInRangeOfPlayer() || AreEnemyRidersInRangeOfPlayer();
		}
	}
}
