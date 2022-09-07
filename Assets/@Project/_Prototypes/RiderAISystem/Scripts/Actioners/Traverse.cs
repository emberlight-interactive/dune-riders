using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(RichAI))]
	public class Traverse : Actioner // todo: [Reminder] when we populate map and leave some areas unspawned it will leave holes in the path finding
	{
		Coroutine activeAction;
		RichAI pathfinder;
		Vector3 finalDestination;
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		void Awake() {
			pathfinder = GetComponent<RichAI>();
		}

		public override void StartAction()
		{
			if (!currentlyActive) {
				CalculateFinalDestination();
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
				pathfinder.destination = CalculatePositionAlongPathToFinalDestination();
				pathfinder.SearchPath();
				yield return new WaitForSeconds(4f);
			}
		}

		void CalculateFinalDestination() {
			if (finalDestination == Vector3.zero) finalDestination = transform.position + (transform.forward * 3000);
		}

		Vector3 CalculatePositionAlongPathToFinalDestination(float distance = 100) {
			return transform.position + (finalDestination - transform.position).normalized * distance;
		}
	}
}
