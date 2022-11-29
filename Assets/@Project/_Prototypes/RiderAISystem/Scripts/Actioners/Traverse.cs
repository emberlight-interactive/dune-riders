using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DuneRiders.AI;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(RichAI))]
	[RequireComponent(typeof(RiderSpeedState))]
	public class Traverse : Actioner
	{
		Coroutine activeAction;
		RiderSpeedState riderSpeedState;
		RichAI pathfinder;
		Vector3 finalDestination;
		float originalSpeed;
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		void Awake() {
			pathfinder = GetComponent<RichAI>();
			riderSpeedState = GetComponent<RiderSpeedState>();
			originalSpeed = pathfinder.maxSpeed;
		}

		public override void StartAction()
		{
			if (!currentlyActive) {
				CalculateFinalDestination();
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
				pathfinder.destination = CalculatePositionAlongPathToFinalDestination();
				pathfinder.SearchPath();
				yield return new WaitForSeconds(4f);
			}
		}

		void SetTraverseSpeed() {
			pathfinder.maxSpeed = riderSpeedState.traverseSpeed;
		}

		void RevertToOriginalSpeed() {
			pathfinder.maxSpeed = originalSpeed;
		}

		void CalculateFinalDestination() {
			if (finalDestination == Vector3.zero) finalDestination = transform.position + (transform.forward * 3000);
		}

		Vector3 CalculatePositionAlongPathToFinalDestination(float distance = 300) {
			return transform.position + (finalDestination - transform.position).normalized * distance;
		}
	}
}
