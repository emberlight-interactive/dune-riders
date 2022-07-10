using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders {
	public class EnemyRiderAISpawner : MonoBehaviour
	{
		[SerializeField] Transform player;
		[SerializeField] SpawnFormation formation;
		[SerializeField] float minSpawnTimeInSeconds = 180;
		[SerializeField] float maxSpawnTimeInSeconds = 1200;
		[SerializeField] SpawnDifficulty spawnDifficulty = SpawnDifficulty.Medium;
		[SerializeField, ReadOnly] int maxRiders = 10;
		[SerializeField] List<GameObject> ridersToSpawn = new List<GameObject>();
		[SerializeField] float distanceMultiplier = 1.0f;
		[SerializeField] bool spawnImmediately = false;

		enum Side {Left, Right};
		enum SpawnDifficulty {VeryEasy, Easy, Medium, Hard};

		void Start() {
			StartCoroutine(Spawner());
		}

		// todo: How to delay while battle takes place??? //
		IEnumerator Spawner() {
			if (spawnImmediately) SpawnBanditsRealtiveToPlayer();
			while (true) {
				yield return new WaitForSeconds(GetWaitTimeInSeconds());
				SpawnBanditsRealtiveToPlayer();
			}
		}

		void SpawnBanditsRealtiveToPlayer() {
			float randomizer = Random.Range(0f, 1f);
			int numberOfRidersToSpawn = NumberOfRidersToSpawn();
				Side spawnSide;

				if (randomizer > 0.5f) {
					spawnSide = Side.Right;
				} else {
					spawnSide = Side.Left;
				}

				var spawnPosition = GetSpawnPosition(spawnSide);
				var spawnRotation = GetSpawnRotation(spawnSide);

				var formationInstance = SimplePool.Spawn(formation.gameObject, spawnPosition, spawnRotation).GetComponent<SpawnFormation>();

				for (int i = 0; i < numberOfRidersToSpawn; i++) {
					SimplePool.Spawn(
						ridersToSpawn[Random.Range(0, ridersToSpawn.Count)],
						formationInstance.formationPositions[i].transform.position,
						spawnRotation
					);
				}

				SimplePool.Despawn(formationInstance.gameObject);
		}

		float GetWaitTimeInSeconds() {
			float randomizer = Random.Range(0f, 1f);
			return ((maxSpawnTimeInSeconds - minSpawnTimeInSeconds) * randomizer) + minSpawnTimeInSeconds;
		}

		Vector3 GetSpawnPosition(Side side) {
			var rightOrLeftVector = (side == Side.Left ? -player.right : player.right);

			return player.position + (player.forward * 400 * distanceMultiplier) + (rightOrLeftVector * 200 * distanceMultiplier);
		}

		Quaternion GetSpawnRotation(Side side) {
			var yAngleModifier = (side == Side.Left ? 90 : -90);
			var rotation = player.rotation;
			rotation.eulerAngles = new Vector3(
				rotation.eulerAngles.x,
				rotation.eulerAngles.y + yAngleModifier,
				rotation.z
			);
			return rotation;
		}

		int NumberOfRidersToSpawn() {
			switch (spawnDifficulty) {
				case SpawnDifficulty.VeryEasy:
					return (int) ((float) maxRiders * 0.2f);
				case SpawnDifficulty.Easy:
					return (int) ((float) maxRiders * 0.4f);
				case SpawnDifficulty.Medium:
					return (int) ((float) maxRiders * 0.7f);
				case SpawnDifficulty.Hard:
					return (int) ((float) maxRiders * 1f);
				default:
					throw new System.ArgumentOutOfRangeException();
			}
		}
	}
}
