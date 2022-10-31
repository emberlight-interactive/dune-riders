using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DuneRiders.RiderAI;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.BanditSpawnerSystem {
	[RequireComponent(typeof(EnemiesInRangeOfPlayer))]
	public class EnemyRiderAISpawner : MonoBehaviour, IPersistent
	{
		EnemiesInRangeOfPlayer enemiesInRangeOfPlayer;

		[SerializeField] Transform player;
		[SerializeField] SpawnFormation formation;
		[SerializeField] float minSpawnTimeInSeconds = 180;
		[SerializeField] float maxSpawnTimeInSeconds = 1200;
		[SerializeField] SpawnDifficulty spawnDifficulty = SpawnDifficulty.Medium;
		[SerializeField] bool getSpawnDifficultyFromPositions = false;
		[SerializeField] List<SpawnerDifficultyPos> spawnerDifficultyPositions = new List<SpawnerDifficultyPos>();
		[SerializeField, ReadOnly] int maxRiders = 10;
		[SerializeField] EnemyRiderInstanceBuilder enemyRiderInstanceBuilder;
		[SerializeField] float distanceMultiplier = 1.0f;
		[SerializeField] bool spawnImmediately = false;
		[SerializeField] float startUpDelay = 0f;
		[SerializeField] UnityEvent roamingEnemyBanditsSpawned = new UnityEvent();
		public bool DisablePersistence { get => false; }

		enum Side {Left, Right};
		public enum SpawnDifficulty {VeryEasy, Easy, Medium, Hard};

		void Awake() {
			enemiesInRangeOfPlayer = GetComponent<EnemiesInRangeOfPlayer>();
		}

		void Start() {
			StartCoroutine(Spawner());
		}

		IEnumerator Spawner() {
			yield return new WaitForSeconds(startUpDelay);
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

			roamingEnemyBanditsSpawned.Invoke();
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
				var riderToSpawn = (rider == null) ? enemyRiderInstanceBuilder.GetRandomRider().gameObject : rider;
				var go = Instantiate(
					riderToSpawn,
					formationInstance.formationPositions[i].transform.position,
					rot
				);
				BubbleGameObjectToActiveScene.BubbleUp(go);
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
			switch (GetSpawnDifficulty()) {
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

		public void Save(IPersistenceUtil persistUtil) {}

        public void Load(IPersistenceUtil persistUtil) {
			// Spawn immediately typically means "when the game first starts"
			spawnImmediately = false;
		}

		SpawnDifficulty GetSpawnDifficulty() {
			if (!getSpawnDifficultyFromPositions) return spawnDifficulty;
			else {
				(float distance, SpawnDifficulty spawnDifficulty) closestSpawnPosition = (default(float), default(SpawnDifficulty));
				foreach (var spawnPos in spawnerDifficultyPositions) {
					if (closestSpawnPosition == default((float, SpawnDifficulty))) {
						closestSpawnPosition.distance = Vector3.Distance(player.position, spawnPos.transform.position);
						closestSpawnPosition.spawnDifficulty = spawnPos.spawnDifficulty;
					} else {
						var distance = Vector3.Distance(player.position, spawnPos.transform.position);
						if (distance < closestSpawnPosition.distance) {
							closestSpawnPosition.distance = distance;
							closestSpawnPosition.spawnDifficulty = spawnPos.spawnDifficulty;
						}
					}
				}

				return closestSpawnPosition.spawnDifficulty;
			}
		}
	}
}
