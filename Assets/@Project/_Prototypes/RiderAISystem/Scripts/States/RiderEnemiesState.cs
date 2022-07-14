using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;
using DuneRiders.OutpostAI.Traits;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Rider))]
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(AllActiveTurretsState))]
	public class RiderEnemiesState : MonoBehaviour
	{
		Rider rider;
		AllActiveRidersState allActiveRidersState;
		AllActiveTurretsState allActiveTurretsState;

		void Awake() {
			rider = GetComponent<Rider>();
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			allActiveTurretsState = GetComponent<AllActiveTurretsState>();
		}

		public Transform GetClosestEnemyTransform(bool convertTurretTransformsToTheirOutpostParent = true) {
			var closestTurretTransform = GetClosestEnemyTurretTransform(convertTurretTransformsToTheirOutpostParent);
			var closestRiderTransform = GetClosestRiderTransform();

			if (closestRiderTransform != null && closestTurretTransform != null) {
				if (Vector3.Distance(transform.position, closestRiderTransform.position) < Vector3.Distance(transform.position, closestTurretTransform.position)) return closestRiderTransform;
				return closestTurretTransform;
			} else if (closestRiderTransform == null && closestTurretTransform == null) {
				return null;
			} else {
				return closestRiderTransform ?? closestTurretTransform;
			}
		}

		public Transform GetClosestEnemyTurretTransform(bool convertTurretTransformsToTheirOutpostParent = false) {
			var closestEnemyTurret = allActiveTurretsState.GetClosestTurretOfAllegiance(rider.enemyAllegiance);
			if (closestEnemyTurret) {
				if (convertTurretTransformsToTheirOutpostParent) {
					var outpost = closestEnemyTurret.GetComponentInParent<Outpost>();
					if (outpost != null) return outpost.transform;
				}

				return closestEnemyTurret.transform;
			}

			return null;
		}

		public Transform GetClosestRiderTransform() {
			var allEnemyRiders = allActiveRidersState.GetAllRidersOfAllegiance(rider.enemyAllegiance);
			if (allEnemyRiders.Count > 0) {
				return allActiveRidersState.GetClosestRiderFromList(allEnemyRiders).transform;
			}

			return null;
		}
	}
}
