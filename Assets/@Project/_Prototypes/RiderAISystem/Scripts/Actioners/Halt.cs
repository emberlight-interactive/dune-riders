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
	[RequireComponent(typeof(UniqueIdentifier))]
	public class Halt : Actioner, IPersistent // todo: Make the halt put down an invisible formation prefab that has fpf and persist the fpf position and the riders target node in that prefab
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
				StartCoroutine(DisablePathfindingWhenDestinationReached());
			}

			_currentlyActive = true;
		}

		public override void EndAction() {
			StopAllCoroutines();
			pathfinder.enabled = true;
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

		IEnumerator DisablePathfindingWhenDestinationReached() {
			yield return null;

			while (true) {
				if (pathfinder.reachedEndOfPath) {
					pathfinder.enabled = false;
				}

				yield return new WaitForSeconds(0.5f);
			}
		}

		public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save(GetPersistenceKey(), new HaltSerializable {
				destinationPositionDifference = pathfinder.destination - transform.position,
				_currentlyActive = this._currentlyActive,
			});
		}

        public void Load(IPersistenceUtil persistUtil) {
			var loadedHalt = persistUtil.Load<HaltSerializable>(GetPersistenceKey());
			destinationPositionDifference = loadedHalt.destinationPositionDifference;

			if (loadedHalt._currentlyActive) useDestinationPositionDifference = true;
		}

		string GetPersistenceKey() {
			return $"HaltActioner-{GetComponent<UniqueIdentifier>().uniqueIdentifier}";
		}
	}
}
