using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DuneRiders.AI;
using DuneRiders.RiderAI.Shared;
using DuneRiders.RiderAI.State;
using DuneRiders.OutpostAI.Traits;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(RiderEnemiesState))]
	[RequireComponent(typeof(RichAI))]
	public class Charge : Actioner
	{
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		RiderEnemiesState riderEnemiesState;
		Coroutine activeAction;
		RichAI pathfinder;
		float movementLongevityMultiplier = 1f;

		void Awake() {
			pathfinder = GetComponent<RichAI>();
			riderEnemiesState = GetComponent<RiderEnemiesState>();
		}

		public override void StartAction()
		{
			if (!currentlyActive) {
				activeAction = StartCoroutine(Action());
			}
			_currentlyActive = true;
		}

		public override void EndAction() {
			if (currentlyActive) {
				StopCoroutine(activeAction);
			}

			_currentlyActive = false;
		}

		IEnumerator Action() {
			while (true) {
				var closestEnemyTransform = riderEnemiesState.GetClosestEnemyTransform(true, true);
				if (closestEnemyTransform == null) closestEnemyTransform = riderEnemiesState.GetClosestEnemyStructureTransform();

				if (closestEnemyTransform) {
					if (IsTypeOfTransformAnOutpost(closestEnemyTransform)) movementLongevityMultiplier = 2f;
					else movementLongevityMultiplier = 1f;

					pathfinder.destination = DetermineBestAttackPosition(closestEnemyTransform);
					pathfinder.SearchPath();
				}

				yield return new WaitForSeconds(4f * movementLongevityMultiplier);
			}
		}

		bool IsTypeOfTransformAnOutpost(Transform transform) {
			if (transform.GetComponent<Outpost>()) return true;
			return false;
		}

		/// todo: Add condition to keep battle within range of player
		/// todo: When far away literally charge them
		Vector3 DetermineBestAttackPosition(Transform positionOfEnemy) {
			var angleOfEnemyFromDirectionOfTravel = UtilityMethods.GetAngleOfTargetFromCurrentDirection(transform, positionOfEnemy.position);

			float newAngleOfTravel = 0;
			float dist = 60 * movementLongevityMultiplier;

			if (angleOfEnemyFromDirectionOfTravel > 60 && angleOfEnemyFromDirectionOfTravel < 180) {
				// Strafe more right
				newAngleOfTravel = Random.Range(80f, 120f);
			} else if (angleOfEnemyFromDirectionOfTravel < 300 && angleOfEnemyFromDirectionOfTravel >= 180) {
				// Strafe more left
				newAngleOfTravel = Random.Range(200f, 240f);
			} else if (angleOfEnemyFromDirectionOfTravel > 340) {
				// Strafe more right
				newAngleOfTravel = Random.Range(340f, 343f);
			} else if (angleOfEnemyFromDirectionOfTravel < 20) {
				// Strafe more left
				newAngleOfTravel = Random.Range(17f, 20f);
			}

			var q = Quaternion.AngleAxis(newAngleOfTravel, Vector3.up);
   			var newPosition = transform.position + q * transform.forward * dist;

			return newPosition;
		}
	}
}
