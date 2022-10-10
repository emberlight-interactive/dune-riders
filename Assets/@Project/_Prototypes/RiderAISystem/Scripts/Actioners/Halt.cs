using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.Shared.PersistenceSystem;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.State;
using Pathfinding;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(RichAI))]
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(Rider))]
	public class Halt : Actioner, IPersistent
	{
		[Serializable]
		class HaltSerializable {
			public Vector3 destinationPositionDifference;
			public bool _currentlyActive;
		}

		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}
		public bool DisablePersistence { get => false; }
		string persistenceKey = "HaltActioner";

		[SerializeField] Formation haltFormation;
		Player player;
		RichAI pathfinder;
		AllActiveRidersState allActiveRiders;
		Rider rider;

		Vector3 destinationPositionDifference;
		bool useDestinationPositionDifference = false;

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

			try {
				Vector3 currentHaltPosition;

				if (!useDestinationPositionDifference) {
					currentHaltPosition = formationPositions[positionOfThisRiderInGlobalIndex].transform.position;
				} else {
					useDestinationPositionDifference = false;
					currentHaltPosition = transform.position + destinationPositionDifference;
				}

				pathfinder.destination = currentHaltPosition;
				pathfinder.SearchPath();
			} catch (System.ArgumentOutOfRangeException exception) {
				Debug.LogWarning("This rider is not moving for now as it was unable to find a suitable position to halt: " + exception);
			}

			SimplePool.Despawn(formation);
		}

		public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save(persistenceKey, new HaltSerializable {
				destinationPositionDifference = pathfinder.destination - transform.position,
				_currentlyActive = this._currentlyActive,
			});
		}

        public void Load(IPersistenceUtil persistUtil) {
			var loadedHalt = persistUtil.Load<HaltSerializable>(persistenceKey);
			destinationPositionDifference = loadedHalt.destinationPositionDifference;

			if (loadedHalt._currentlyActive) useDestinationPositionDifference = true;
		}
	}
}
