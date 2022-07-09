using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DuneRiders.RiderAI.Shared;
using DuneRiders.RiderAI.State;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(Rider))]
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(WorldSpaceState))]
	[RequireComponent(typeof(RichAI))]
	public class ChargeAndAttack : Actioner
	{
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		Coroutine activeAction;
		Rider rider;
		RichAI pathfinder;
		WorldSpaceState worldSpaceState;
		AllActiveRidersState allActiveRidersState;
		[SerializeField] Turret turret;

		void Awake() {
			pathfinder = GetComponent<RichAI>();
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			worldSpaceState = GetComponent<WorldSpaceState>();
			rider = GetComponent<Rider>();
			if (!turret) Debug.LogError("Please assign a turret");
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
				var allEnemyRiders = allActiveRidersState.GetAllRidersOfAllegiance(rider.enemyAllegiance);
				if (allEnemyRiders.Count > 0) {
					var enemyRiderToAttack = allActiveRidersState.GetClosestRiderFromList(allEnemyRiders);
					pathfinder.destination = DetermineBestAttackPosition(enemyRiderToAttack.rider.gameObject.transform);
					pathfinder.SearchPath();
					turret.FireOnTarget(enemyRiderToAttack.rider);
				}

				yield return new WaitForSeconds(4f);
			}
		}

		/// todo: Add condition to keep battle within range of player
		Vector3 DetermineBestAttackPosition(Transform positionOfEnemy) {
			var angleOfEnemyFromDirectionOfTravel = UtilityMethods.GetAngleOfTargetFromCurrentDirection(transform, positionOfEnemy.position);

			float newAngleOfTravel = 0;
			float dist = 60;

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
