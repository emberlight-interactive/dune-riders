using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GatheringSystem {
	public class Breakable : MonoBehaviour
	{
		[SerializeField] Transform[] gatherableSpawnLocations;
		[SerializeField] Gatherable gatherableToSpawn;

		void OnDisable() {
			for (int i = 0; i < gatherableSpawnLocations.Length; i++) {
				SimplePool.Spawn(gatherableToSpawn.gameObject, gatherableSpawnLocations[i].position, gatherableToSpawn.transform.rotation);
			}
		}

		void OnTriggerEnter(Collider other) {
			gameObject.SetActive(false);
		}
	}
}
