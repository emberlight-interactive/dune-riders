using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DuneRiders.HomeVillageSystem;
using DuneRiders.RiderAI.Shared;

namespace DuneRiders.VillageMigrationSystem {
	public class VillageMigrationManager : MonoBehaviour
	{
		[SerializeField] HomeVillageInteractionTarget homeVillageInteractionTarget;
		[SerializeField] VillageMigrationWaypoint currentWaypoint;
		[SerializeField] Transform player;
		[SerializeField] float distanceFromPlayerForJump = 100f;
		[SerializeField] UnityEvent winEvent;

		bool isCurrentlyMigrating = false;

		void OnEnable() {
			if (currentWaypoint.nextWaypoint == null) {
				SetNextJumpIsWinOnVillageInteractionTarget();
			}

			if (isCurrentlyMigrating) {
				StartMigration();
			}
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		public void StartMigration() {
			if (currentWaypoint.nextWaypoint == null) TriggerWinEvent();
			else {
				isCurrentlyMigrating = true;
				StartCoroutine(WatchForMigrateOpportunity());
			}
		}

		IEnumerator WatchForMigrateOpportunity() {
			while (true) {
				if (PlayerIsIgnorantOfVillagePosition()) {
					Migrate();
					break;
				}

				yield return new WaitForSeconds(1f);
			}

		}

		void Migrate() {
			JumpToNewLocation();
			homeVillageInteractionTarget.MigrationFinished();
			isCurrentlyMigrating = false;

			if (currentWaypoint.nextWaypoint == null) {
				SetNextJumpIsWinOnVillageInteractionTarget();
			}
		}

		void JumpToNewLocation() {
			currentWaypoint = currentWaypoint.nextWaypoint;
			transform.position = currentWaypoint.transform.position;
		}

		void SetNextJumpIsWinOnVillageInteractionTarget() {
			homeVillageInteractionTarget.nextMigrationTriggersWinCondition = true;
		}

		bool PlayerIsIgnorantOfVillagePosition() {
			return (IsPlayerFarAwayFromVillage() && PlayerIsFacingAwayFromVillage());
		}

		bool PlayerIsFacingAwayFromVillage() {
			var angle = UtilityMethods.GetAngleOfTargetFromCurrentDirection(player, transform.position);

			return (angle < 240 && angle > 130);
		}

		bool IsPlayerFarAwayFromVillage() {
			return Vector3.Distance(player.position, transform.position) > distanceFromPlayerForJump;
		}

		void TriggerWinEvent() {
			winEvent.Invoke();
		}
	}
}
