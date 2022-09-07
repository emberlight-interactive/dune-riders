using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	public class RespawnableEntityTester : MonoBehaviour
	{
		[Serializable]
		class Respawnable {
			public Vector3 position;
			public GameObject entity;
		}

		[SerializeField] List<Respawnable> respawnables;
		[SerializeField] List<GameObject> spawnedRespawnables;

		void Start() {
			SpawnAllRespawnables();
			StartCoroutine(ReSpawnRespawnables());
		}

		void SpawnAllRespawnables() {
			foreach (var respawnable in respawnables) {
				spawnedRespawnables.Add(Instantiate(respawnable.entity, respawnable.position, Quaternion.identity));
			}
		}

		void DestroyAllRespawnables() {
			foreach (var respawnable in spawnedRespawnables) {
				Destroy(respawnable);
			}
		}

		IEnumerator ReSpawnRespawnables() {
			yield return new WaitForSeconds(6f);
			DestroyAllRespawnables();
			yield return new WaitForSeconds(0.5f);
			SpawnAllRespawnables();
		}
	}
}
