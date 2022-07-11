using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(HealthState))]
	[RequireComponent(typeof(Rider))]
	public class InCombatState : MonoBehaviour
	{
		[ReadOnly] public bool inCombat = false;

		AllActiveRidersState allActiveRidersState;
		HealthState healthState;
		Rider rider;

		void Awake()
		{
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			healthState = GetComponent<HealthState>();
			rider = GetComponent<Rider>();
		}

		void Start() {
			StartCoroutine(UpdateInCombatState());
		}

		IEnumerator UpdateInCombatState() {
			while (true) {
				if (AreThereAnyEnemiesLeft()) {
					if (HaveITakenDamage() || AreAnyOfMyFriendsInCombat() || AreThereAnyEnemiesInFiringRangeOfMe()) {
						inCombat = true;
					}
				} else {
					inCombat = false;
				}


				yield return new WaitForSeconds(2f);
			}
		}

		bool AreThereAnyEnemiesLeft() {
			return allActiveRidersState.GetAllRidersOfAllegiance(rider.enemyAllegiance).Count > 0;
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
			var closestEnemyRider = allActiveRidersState.GetClosestRiderFromList(allActiveRidersState.GetAllRidersOfAllegiance(rider.enemyAllegiance));
			if (Vector3.Distance(transform.position, closestEnemyRider.transform.position) < 200) return true;
			return false;
		}

		bool HaveITakenDamage() {
			if (healthState.health < 100) return true;
			return false;
		}
	}
}
