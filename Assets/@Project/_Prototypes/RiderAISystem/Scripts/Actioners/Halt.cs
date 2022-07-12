using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.State;
using Pathfinding;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(RichAI))]
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(Rider))]
	public class Halt : Actioner
	{
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		[SerializeField] Formation haltFormation;
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
			}

			_currentlyActive = true;
		}

		public override void EndAction() {
			_currentlyActive = false;
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
