using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(Rider))]
	public class Gunner : Actioner
	{
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		Coroutine activeAction;
		[SerializeField] Turret turret;
		Rider rider;
		AllActiveRidersState allActiveRiders;

		void Awake() {
			rider = GetComponent<Rider>();
			allActiveRiders = GetComponent<AllActiveRidersState>();
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
				var allEnemyRiders = allActiveRiders.GetAllRidersOfAllegiance(rider.enemyAllegiance);
				if (allEnemyRiders.Count > 0) {
					var enemyRiderToAttack = allActiveRiders.GetClosestRiderFromList(allEnemyRiders);
					turret.FireOnTarget(enemyRiderToAttack.rider);
				}
				yield return new WaitForSeconds(4f);
			}
		}
	}
}
