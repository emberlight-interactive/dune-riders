using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.HomeVillageSystem {
	public class TestMigration : MonoBehaviour
	{
		[SerializeField] HomeVillageInteractionTarget homeVillageInteractionTarget;

		public void StartMigration() {
			Debug.Log("Start Migration");
			StartCoroutine(StartMigrationProcess());
		}

		IEnumerator StartMigrationProcess() {
			yield return new WaitForSeconds(10f);
			homeVillageInteractionTarget.MigrationFinished();
			Debug.Log("Migration finished");
		}
	}
}
