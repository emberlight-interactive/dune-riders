using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(AllActiveTurretsState))]
	[RequireComponent(typeof(AllNearbyOutpostsState))]
	[RequireComponent(typeof(HealthState))]
	[RequireComponent(typeof(Rider))]
	[RequireComponent(typeof(PlayerFleeingState))]
	public class InCombatState : MonoBehaviour
	{
		[ReadOnly] public bool inCombat = false;

		AllActiveRidersState allActiveRidersState;
		AllActiveTurretsState allActiveTurretsState;
		AllNearbyOutpostsState allNearbyOutpostsState;
		PlayerFleeingState playerFleeingState;
		HealthState healthState;
		Rider rider;
		float firingRangeOfThisRider = 200;
		float detectRange = 400;
		float healthFromLastCheck = 0;

		void Awake()
		{
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			allActiveTurretsState = GetComponent<AllActiveTurretsState>();
			allNearbyOutpostsState = GetComponent<AllNearbyOutpostsState>();
			playerFleeingState = GetComponent<PlayerFleeingState>();
			healthState = GetComponent<HealthState>();
			rider = GetComponent<Rider>();

			healthFromLastCheck = healthState.health;
		}

		void Start() {
			StartCoroutine(UpdateInCombatState());
		}

		IEnumerator UpdateInCombatState() {
			while (true) {
				if (AreThereAnyEnemiesLeft() && !playerFleeingState.PlayerFleeing) {
					if (HaveITakenDamage() || AreAnyOfMyFriendsInCombat() || AreThereAnyEnemiesInFiringRangeOfMe()) {
						inCombat = true;
					}
				} else if (AreAnyEnemyStructuresInRange()) {
					inCombat = true;
				} else {
					inCombat = false;
				}


				yield return new WaitForSeconds(2f);
			}
		}

		bool AreThereAnyEnemiesLeft() {
			return AreThereAnyEnemyRidersLeft() || AreThereAnyEnemyTurretsLeft();
		}

		bool AreThereAnyEnemyRidersLeft() {
			return allActiveRidersState.GetAllRidersOfAllegiance(rider.enemyAllegiance).Count > 0;
		}

		bool AreThereAnyEnemyTurretsLeft() {
			return allActiveTurretsState.GetAllTurretsOfAllegiance(rider.enemyAllegiance).Count > 0;
		}

		bool AreAnyOfMyFriendsInCombat() {
			var allFriendlyRiders = allActiveRidersState.GetAllRidersOfAllegiance(rider.allegiance);

			foreach (var rider in allFriendlyRiders) {
				var riderCombatState = rider.transform.gameObject.GetComponent<InCombatState>();
				if (riderCombatState != null && riderCombatState.inCombat) {
					return true;
				}
			}

			return false;
		}

		bool AreThereAnyEnemiesInFiringRangeOfMe() {
			if (AreThereAnyEnemyRidersInFiringRangeOfMe() || AreThereAnyEnemyTurretsInFiringRangeOfMe()) return true;
			return false;
		}

		bool AreThereAnyEnemyRidersInFiringRangeOfMe() {
			var allEnemyRiders = allActiveRidersState.GetAllRidersOfAllegiance(rider.enemyAllegiance);
			if (allEnemyRiders.Count > 0) {
				var closestEnemyRider = allActiveRidersState.GetClosestRiderFromList(allEnemyRiders);
				if (Vector3.Distance(transform.position, closestEnemyRider.transform.position) < firingRangeOfThisRider) return true;
			}

			return false;
		}

		bool AreThereAnyEnemyTurretsInFiringRangeOfMe() {
			var closestEnemyTurret = allActiveTurretsState.GetClosestTurretOfAllegiance(rider.enemyAllegiance);
			if (closestEnemyTurret) {
				if (Vector3.Distance(transform.position, closestEnemyTurret.transform.position) < firingRangeOfThisRider) return true;
			}

			return false;
		}


		bool HaveITakenDamage() {
			var haveITakenDamage = healthState.health < healthFromLastCheck;
			healthFromLastCheck = healthState.health;
			return haveITakenDamage;
		}

		bool AreAnyEnemyStructuresInRange() {
			var closestEnemyOutpost = allNearbyOutpostsState.GetClosestOutpostOfAllegiance(rider.enemyAllegiance);
			if (closestEnemyOutpost) {
				if (Vector3.Distance(transform.position, closestEnemyOutpost.transform.position) < detectRange) return true;
			}
			return false;
		}
	}
}
