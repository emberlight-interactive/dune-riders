using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.State;
using DuneRiders.OutpostAI.Traits;
using DuneRiders.RiderAI.Actioners;

namespace DuneRiders.OutpostAI.Actioners {
	[RequireComponent(typeof(OutpostTurret))]
	[RequireComponent(typeof(AllActiveRidersState))]
	public class Gunner : Actioner
	{
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		Coroutine activeAction;
		[SerializeField] Turret turret;
		AllActiveRidersState allActiveRidersState;
		OutpostTurret outpostTurret;

		void Awake() {
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			outpostTurret = GetComponent<OutpostTurret>();
		}

		void OnEnable() {
			if (currentlyActive) StartCoroutine(Action());
		}

		void OnDisable() {
			StopAllCoroutines();
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
				var closestEnemyTransform = GetClosestEnemyRiderTransform();
				if (closestEnemyTransform) {
					turret.FireOnTarget(closestEnemyTransform);
				}

				yield return new WaitForSeconds(4f);
			}
		}

		Transform GetClosestEnemyRiderTransform() {
			var allEnemyRiders = allActiveRidersState.GetAllRidersOfAllegiance(outpostTurret.enemyAllegiance);
			if (allEnemyRiders.Count > 0) {
				return allActiveRidersState.GetClosestRiderFromList(allEnemyRiders).transform;
			}

			return null;
		}
	}
}
