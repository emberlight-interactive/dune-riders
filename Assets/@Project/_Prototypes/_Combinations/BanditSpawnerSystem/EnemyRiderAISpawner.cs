using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.BanditSpawnerSystem {
	[RequireComponent(typeof(EnemiesInRangeOfPlayer))]
	public class EnemyRiderAISpawner : MonoBehaviour
	{
		EnemiesInRangeOfPlayer enemiesInRangeOfPlayer;

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

		void Awake() {
			enemiesInRangeOfPlayer = GetComponent<EnemiesInRangeOfPlayer>();
		}

		void Start() {
			StartCoroutine(Spawner());
		}

		IEnumerator Spawner() {
			if (spawnImmediately) SpawnBanditsRealtiveToPlayer();
			while (true) {
				yield return new WaitForSeconds(GetWaitTimeInSeconds());
				if (!enemiesInRangeOfPlayer.AreEnemiesInRangeOfPlayer()) SpawnBanditsRealtiveToPlayer();
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

			SpawnRiders(spawnPosition, spawnRotation, numberOfRidersToSpawn);
		}

		public void SpawnDefenseBandits(Transform positionToDefend, GameObject defenseRider = null) {
			var spawnPosition = GetSpawnPosition(positionToDefend);
			var toTheRescuePos = positionToDefend.position - spawnPosition;
			var spawnRotation = Quaternion.LookRotation(toTheRescuePos);
			spawnRotation.eulerAngles = new Vector3(
				0,
				spawnRotation.eulerAngles.y,
				0
			);

			SpawnRiders(spawnPosition, spawnRotation, maxRiders, defenseRider);
		}

		void SpawnRiders(Vector3 pos, Quaternion rot, int numberOfRiders, GameObject rider = null) {
			var formationInstance = SimplePool.Spawn(formation.gameObject, pos, rot).GetComponent<SpawnFormation>();

			for (int i = 0; i < numberOfRiders; i++) {
				var riderToSpawn = (rider == null) ? ridersToSpawn[Random.Range(0, ridersToSpawn.Count)] : rider;
				Instantiate(
					riderToSpawn,
					formationInstance.formationPositions[i].transform.position,
					rot
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

			return player.position + (player.forward * 400 * distanceMultiplier) + (rightOrLeftVector * 400 * distanceMultiplier);
		}

		Vector3 GetSpawnPosition(Transform relativeTo) {
			return relativeTo.position + (relativeTo.forward * 400);
		}

		Quaternion GetSpawnRotation(Side side) {
			var yAngleModifier = (side == Side.Left ? 90 : -90);
			var rotation = player.rotation;
			rotation.eulerAngles = new Vector3(
				rotation.eulerAngles.x, // todo: 0?, player might be on an angle and could affect raycasts
				rotation.eulerAngles.y + yAngleModifier,
				rotation.z // todo: 0?, player might be on an angle and could affect raycasts
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
