using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Shared;
using DuneRiders.RiderAI.State;
using DuneRiders.RiderAI.Traits;
using Pathfinding;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(AveragePositionOfRidersState))]
	[RequireComponent(typeof(RichAI))]
	[RequireComponent(typeof(Rider))]
	public class FollowPlayer : Actioner
	{
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		Coroutine activeAction;
		[SerializeField] Formation lineFormationPrefab;
		[SerializeField] Formation columnFormationPrefab;
		Formation lineFormation;
		Formation columnFormation;
		Player player;
		Rider rider;
		AllActiveRidersState allActiveRiders;
		AveragePositionOfRidersState averagePositionOfRiders;
		RichAI pathfinder;

		void Awake() {
			player = FindObjectOfType<Player>();
			if (player == null) Debug.LogError("Object with Player component in scene is required");
			rider = GetComponent<Rider>();
			allActiveRiders = GetComponent<AllActiveRidersState>();
			averagePositionOfRiders = GetComponent<AveragePositionOfRidersState>();
			pathfinder = GetComponent<RichAI>();
			AttachFormationsToPlayer();
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
				pathfinder.destination = FindBestFollowPosition(); // todo: Make halt and attack and follow and attack rotate to face enemy?
				pathfinder.SearchPath();
				yield return new WaitForSeconds(4f);
			}
		}

		void AttachFormationsToPlayer() {
			List<FormationTag> formationTags = new List<FormationTag>();
			formationTags.AddRange(player.GetComponentsInChildren<FormationTag>());

			if (formationTags.Count == 0) {
				(Formation, string)[] formationsToInstantiate = {
					(lineFormationPrefab, "lineFormation"),
					(columnFormationPrefab, "columnFormation"),
				};

				for (int i = 0; i < formationsToInstantiate.Length; i++) {
					GameObject formationGameObject = Instantiate(formationsToInstantiate[i].Item1.gameObject, player.gameObject.transform, false) as GameObject;
					var formationTag = formationGameObject.AddComponent<FormationTag>();
					formationTag.formationName = formationsToInstantiate[i].Item2;
					formationGameObject.transform.localPosition = new Vector3(0, 0, -11);
					formationTags.Add(formationTag);
				}
			}

			foreach (var formationTag in formationTags) {
				if (formationTag.formationName == "lineFormation") lineFormation = formationTag.gameObject.GetComponent<Formation>();
				if (formationTag.formationName == "columnFormation") columnFormation = formationTag.gameObject.GetComponent<Formation>();
			}
		}

		Vector3 FindBestFollowPosition() {
			var numberOfAliveEnemies = allActiveRiders.GetAllRidersOfAllegiance(rider.enemyAllegiance).Count;
			var averagePositionOfEnemy = averagePositionOfRiders.GetAverageWorldPositionOfRiders(rider.enemyAllegiance);
			var angleOfEnemyFromDirectionOfTravel = UtilityMethods.GetAngleOfTargetFromCurrentDirection(player.transform, averagePositionOfEnemy);

			int positionOfThisRiderInGlobalIndex = allActiveRiders.GetAllRidersOfAllegiance(rider.allegiance).FindIndex(
				(riderData) => GameObject.ReferenceEquals(riderData.rider.gameObject, gameObject)
			);

			if (numberOfAliveEnemies > 0 && (angleOfEnemyFromDirectionOfTravel > 330 || angleOfEnemyFromDirectionOfTravel < 30)) {
				return lineFormation.formationPositions[positionOfThisRiderInGlobalIndex].transform.position;
			} else {
				return columnFormation.formationPositions[positionOfThisRiderInGlobalIndex].transform.position;
			}
		}
	}

	class FormationTag : MonoBehaviour {
		public string formationName;
	}
}
