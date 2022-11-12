using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DuneRiders.RiderAI.State;
using DuneRiders.RiderAI.Traits;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(RichAI))]
	[RequireComponent(typeof(Rider))]
	[RequireComponent(typeof(AveragePositionOfEntitiesState))]
	public class Retreat : Actioner
	{
		Coroutine activeAction;
		RichAI pathfinder;
		Rider rider;
		Player player;
		AveragePositionOfEntitiesState averagePositionOfEntitiesState;
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		void Awake() {
			pathfinder = GetComponent<RichAI>();
			averagePositionOfEntitiesState = GetComponent<AveragePositionOfEntitiesState>();
			rider = GetComponent<Rider>();
			player = FindObjectOfType<Player>();
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
				pathfinder.destination = GetNewRetreatDestination(200);
				pathfinder.SearchPath();
				yield return new WaitForSeconds(4f);
			}
		}

		Vector3 GetNewRetreatDestination(float distance) {
			var enemyAveragePosition = averagePositionOfEntitiesState.GetAverageWorldPositionOfEntities(rider.enemyAllegiance);
			var directionAwayFromEnemy = (transform.position - enemyAveragePosition).normalized;
			return transform.position + directionAwayFromEnemy * distance;
		}
	}
}
