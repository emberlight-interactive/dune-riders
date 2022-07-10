using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.State;
using Pathfinding;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(RichAI))]
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(Rider))]
	public class HaltAndAttack : Actioner
	{
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		Coroutine activeAction;
		[SerializeField] Formation haltFormation; // Increase formation and spawn formation distances
		[SerializeField] Turret turret;
		Player player;
		RichAI pathfinder;
		AllActiveRidersState allActiveRiders;
		Rider rider;

		void Awake() {
			player = FindObjectOfType<Player>();
			if (player == null) Debug.LogError("Object with player trait in scene is required for this Actioner");
			pathfinder = GetComponent<RichAI>();
			allActiveRiders = GetComponent<AllActiveRidersState>();
			rider = GetComponent<Rider>();
		}

		public override void StartAction()
		{
			if (!currentlyActive) {
				MoveToHaltPosition();
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

		void MoveToHaltPosition() {
			var formation = SimplePool.Spawn(haltFormation.gameObject, player.transform.position, player.transform.rotation);
			var formationPositions = formation.GetComponent<Formation>().formationPositions;
			int positionOfThisRiderInGlobalIndex = allActiveRiders.GetAllRidersOfAllegiance(rider.allegiance).FindIndex(
				(riderData) => GameObject.ReferenceEquals(riderData.rider.gameObject, gameObject)
			);

			pathfinder.destination = formationPositions[positionOfThisRiderInGlobalIndex].transform.position;
			pathfinder.SearchPath();

			SimplePool.Despawn(formation);
		}
	}
}
