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
	public class FollowPlayerAndAttack : Actioner
	{
		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		Coroutine activeAction;
		Player player;
		[SerializeField] Formation lineFormationPrefab;
		[SerializeField] Formation columnFormationPrefab;
		[SerializeField] Turret turret;
		Formation lineFormation;
		Formation columnFormation;
		AllActiveRidersState allActiveRiders;
		AveragePositionOfRidersState averagePositionOfRiders;
		RichAI pathfinder;

		void Awake() {
			player = FindObjectOfType<Player>();
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
				var allEnemyRiders = allActiveRiders.GetAllRidersOfAllegiance(Rider.Allegiance.Bandits);
				if (allEnemyRiders.Count > 0) {
					var enemyRiderToAttack = allActiveRiders.GetClosestRiderFromList(allEnemyRiders);
					pathfinder.destination = FindBestFollowPosition();
					pathfinder.SearchPath();
					turret.FireOnTarget(enemyRiderToAttack.rider);
				}
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
					formationGameObject.transform.localPosition = new Vector3(0, 0, -5);
					formationTags.Add(formationTag);
				}
			}

			foreach (var formationTag in formationTags) {
				if (formationTag.formationName == "lineFormation") lineFormation = formationTag.gameObject.GetComponent<Formation>();
				if (formationTag.formationName == "columnFormation") columnFormation = formationTag.gameObject.GetComponent<Formation>();
			}
		}

		Vector3 FindBestFollowPosition() {
			var averagePositionOfEnemy = averagePositionOfRiders.GetAverageWorldPositionOfRiders(Rider.Allegiance.Bandits);
			var angleOfEnemyFromDirectionOfTravel = UtilityMethods.GetAngleOfTargetFromCurrentDirection(transform, averagePositionOfEnemy);

			int positionOfThisRiderInGlobalIndex = allActiveRiders.GetAllRidersOfAllegiance(Rider.Allegiance.Player).FindIndex(
				(riderData) => GameObject.ReferenceEquals(riderData.rider.gameObject, gameObject)
			);

			if (angleOfEnemyFromDirectionOfTravel > 330 || angleOfEnemyFromDirectionOfTravel < 30) {
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
