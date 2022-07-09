using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DuneRiders.RiderAI.State;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(RichAI))]
	[RequireComponent(typeof(AveragePositionOfRidersState))]
	[RequireComponent(typeof(Rider))]
	public class Flee : Actioner
	{
		Coroutine activeAction;
		RichAI pathfinder;
		AveragePositionOfRidersState ridersAveragePosition;
		Rider rider;
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		void Awake() {
			pathfinder = GetComponent<RichAI>();
			ridersAveragePosition = GetComponent<AveragePositionOfRidersState>();
			rider = GetComponent<Rider>();
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
				pathfinder.destination = GetNewFleeDestination(90);
				pathfinder.SearchPath();
				yield return new WaitForSeconds(4f);
			}
		}

		Vector3 GetNewFleeDestination(float distance) {
			var enemyRidersAveragePosition = ridersAveragePosition.GetAverageWorldPositionOfRiders(rider.enemyAllegiance);
			var directionAwayFromEnemy = (transform.position - enemyRidersAveragePosition).normalized;
			return transform.position + directionAwayFromEnemy * distance;
		}
	}
}
