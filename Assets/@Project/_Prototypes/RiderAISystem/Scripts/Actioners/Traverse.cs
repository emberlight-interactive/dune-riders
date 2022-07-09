using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(RichAI))]
	public class Traverse : Actioner
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
				finalDestination = transform.position + (transform.forward * 3000);
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
				pathfinder.destination = transform.position + (finalDestination - transform.position).normalized * 100;
				pathfinder.SearchPath();
				yield return new WaitForSeconds(4f);
			}
		}
	}
}
