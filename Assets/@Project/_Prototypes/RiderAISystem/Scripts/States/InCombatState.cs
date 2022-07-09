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
				if (HaveITakenDamage() || AreAnyOfMyFriendsInCombat() || AreThereAnyEnemiesInFiringRangeOfMe()) {
					inCombat = true;
					yield break;
				}
				yield return new WaitForSeconds(2f);
			}
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
			if (Vector3.Distance(transform.position, closestEnemyRider.transform.position) < 150) return true;
			return false;
		}

		bool HaveITakenDamage() {
			if (healthState.health < 100) return true;
			return false;
		}
	}
}
