using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.RiderAI.State;
using DuneRiders.OutpostAI.Traits;

namespace DuneRiders.OutpostAI.State {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(OutpostTurret))]
	[RequireComponent(typeof(AllActiveRidersState))]
	public class InCombatState : MonoBehaviour
	{
		[ReadOnly] public bool inCombat = false;

		OutpostTurret outpostTurret;
		AllActiveRidersState allActiveRidersState;
		float firingRangeOfThisTurret = 300;

		void Awake()
		{
			outpostTurret = GetComponent<OutpostTurret>();
			allActiveRidersState = GetComponent<AllActiveRidersState>();
		}

		void Start() {
			StartCoroutine(UpdateInCombatState());
		}

		IEnumerator UpdateInCombatState() {
			while (true) {
				if (AreThereAnyEnemiesInFiringRangeOfMe()) {
					inCombat = true;
				} else {
					inCombat = false;
				}

				yield return new WaitForSeconds(2f);
			}
		}

		bool AreThereAnyEnemiesInFiringRangeOfMe() {
			var allEnemyRiders = allActiveRidersState.GetAllRidersOfAllegiance(outpostTurret.enemyAllegiance);
			if (allEnemyRiders.Count > 0) {
				var closestEnemyRider = allActiveRidersState.GetClosestRiderFromList(allEnemyRiders);
				if (Vector3.Distance(transform.position, closestEnemyRider.transform.position) < firingRangeOfThisTurret) return true;
			}

			return false;
		}
	}
}
