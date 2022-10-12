using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DuneRiders.AI;
using DuneRiders.RiderAI.Shared;
using DuneRiders.RiderAI.State;
using DuneRiders.RiderAI.Traits;
using DuneRiders.OutpostAI.Traits;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(RiderEnemiesState))]
	[RequireComponent(typeof(RichAI))]
	[RequireComponent(typeof(RiderSpeedState))]
	[RequireComponent(typeof(Rider))]
	public class Charge : Actioner
	{
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		Rider rider;
		RiderEnemiesState riderEnemiesState;
		RiderSpeedState riderSpeedState;
		Coroutine activeAction;
		RichAI pathfinder;
		float originalSpeed;
		float originalAcceleration;
		float movementLongevityMultiplier = 1f;

		void Awake() {
			rider = GetComponent<Rider>();
			pathfinder = GetComponent<RichAI>();
			riderEnemiesState = GetComponent<RiderEnemiesState>();
			riderSpeedState = GetComponent<RiderSpeedState>();
			originalSpeed = pathfinder.maxSpeed;
			originalAcceleration = pathfinder.acceleration;
		}

		public override void StartAction()
		{
			if (!currentlyActive) {
				SetTraverseSpeed();
				activeAction = StartCoroutine(Action());
			}
			_currentlyActive = true;
		}

		public override void EndAction() {
			if (currentlyActive) {
				RevertToOriginalSpeed();
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

		void SetTraverseSpeed() {
			var riderSpeed = riderSpeedState.GetRiderSpeed(rider.chasisType);
			pathfinder.maxSpeed = riderSpeed.speed;
			pathfinder.acceleration = riderSpeed.acceleration;
		}

		void RevertToOriginalSpeed() {
			pathfinder.maxSpeed = originalSpeed;
			pathfinder.acceleration = originalAcceleration;
		}

		bool IsTypeOfTransformAnOutpost(Transform transform) {
			if (transform.GetComponent<Outpost>()) return true;
			return false;
		}

		Vector3 DetermineBestAttackPosition(Transform positionOfEnemy) {
			// When far away literally charge them
			if (Vector3.Distance(transform.position, positionOfEnemy.position) > 250) return positionOfEnemy.position;

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
