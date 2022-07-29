using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(RiderEnemiesState))]
	public class Gunner : Actioner
	{
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		Coroutine activeAction;
		public Turret turret;
		RiderEnemiesState riderEnemiesState;

		void Awake() {
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
				turret.StopFiring();
			}

			_currentlyActive = false;
		}

		IEnumerator Action() {
			while (true) {
				var closestEnemyTransform = riderEnemiesState.GetClosestEnemyTransform(false, true);
				if (closestEnemyTransform) {
					turret.FireOnTarget(closestEnemyTransform);
				} else {
					var closestEnemyStructureTransform = riderEnemiesState.GetClosestEnemyStructureTransform();
					if (closestEnemyStructureTransform) {
						turret.FireOnTarget(closestEnemyStructureTransform);
					}
				}

				yield return new WaitForSeconds(4f);
			}
		}
	}
}
