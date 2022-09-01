using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.POISystem {
	public class POI : MonoBehaviour
	{
		[System.Serializable]
		public class POIState {
			public string transformHash;
			public List<LootableState> lootableStates;
		}

		[Serializable]
		public class LootableState {
			public bool harvested = false;
			public Vector3 locationPosition;
			public Quaternion locationRotation;
			public GameObject lootable;
			public GameObject spawnedLootable;
		}

		[Serializable]
		public class Lootable {
			public GameObject gameObject;
			public int availabilityValue = 1;
		}

		[SerializeField] List<Lootable> lootables = new List<Lootable>();
		[SerializeField] List<Transform> lootLocations = new List<Transform>();
		[SerializeField] int minimumNumberOfLootables = 3;
		string transformHash;

		POIGlobalState globalState;
		[SerializeField, ReadOnly] POIState state;

		void Awake() {
			transformHash = BuildTransformHash();
			InitializeGlobalState();
			InitializeLocalState();
			SpawnLootables();
		}

		void OnEnable() {
			StartCoroutine(UpdateStateOfLootables());
		}

		void OnDisable() {
			StopAllCoroutines();
			DespawnLootables();
		}

		void FixedUpdate() {
			var str = BuildTransformHash();
		}

		void InitializeGlobalState() {
			POIGlobalState existingGlobalState = FindObjectOfType<POIGlobalState>();
			if (existingGlobalState != null) {
				globalState = existingGlobalState;
				return;
			}

			globalState = new GameObject("POIGlobalState").AddComponent<POIGlobalState>();
		}

		void InitializeLocalState() {
			var poiState = new POIState() {
				transformHash = transformHash,
				lootableStates = CompileLootableStates(),
			};

			globalState.AddState(poiState);
			state = globalState.GetState(transformHash);
		}

		int GetNumberOfLootables() {
			int numberOfLootables;

			if (minimumNumberOfLootables >= lootLocations.Count) {
				numberOfLootables = lootLocations.Count;
			} else {
				var additionalLootables = HashToRandInt(transformHash, lootLocations.Count - minimumNumberOfLootables);
				numberOfLootables = additionalLootables + minimumNumberOfLootables;
			}

			return numberOfLootables;
		}

		List<LootableState> CompileLootableStates() {
			var lootableStates = new List<LootableState>();
			var numberOfLootables = GetNumberOfLootables();

			if (numberOfLootables == 0 || lootables.Count == 0) return lootableStates;

			string currentHash = HashString(transformHash);
			for (int i = 0; i < numberOfLootables; i++) {
				lootableStates.Add(new LootableState {
					lootable = SelectLootable(currentHash).gameObject,
					locationPosition = lootLocations[i].position,
					locationRotation = lootLocations[i].rotation,
				});

				currentHash = HashString(currentHash);
			}

			return lootableStates;
		}

		Lootable SelectLootable(string seed) {
			var combinedAvailabilityValues = lootables.Sum((lootable) => lootable.availabilityValue);
			var randomNumber = HashToRandInt(seed, combinedAvailabilityValues);

			int currentIteratedRange = 0;
			foreach (var lootable in lootables) {
				currentIteratedRange += lootable.availabilityValue;

				if (randomNumber <= currentIteratedRange) return lootable;
			}

			return lootables[0];
		}

		void SpawnLootables() {
			foreach (var lootableState in state.lootableStates) {
				if (lootableState.harvested) continue;

				lootableState.spawnedLootable = SimplePool.Spawn(lootableState.lootable, lootableState.locationPosition, lootableState.locationRotation);
			}
		}

		void DespawnLootables() {
			foreach (var lootableState in state.lootableStates) {
				if (!lootableState.spawnedLootable) continue;

				SimplePool.Despawn(lootableState.spawnedLootable);
			}
		}

		IEnumerator UpdateStateOfLootables() {
			while (true) {
				UpdateHarvestedStateOfLootables();
				yield return new WaitForSeconds(0.3f); // todo: Prevent "destroy" -> save and exit -> load -> destroy loop (very low priority)
			}
		}

		void UpdateHarvestedStateOfLootables() {
			foreach (var lootableState in state.lootableStates) {
				if (lootableState.spawnedLootable == null || lootableState.spawnedLootable.activeSelf == false) {
					lootableState.harvested = true;
				}
			}
		}

		string BuildTransformHash() {
			var transformSum = transform.position.x + transform.position.y + transform.position.z;
			byte[] transformSumBytes = BitConverter.GetBytes(transformSum);

			HashAlgorithm md5 = MD5.Create();
			byte[] result = md5.ComputeHash(transformSumBytes);

			return Encoding.UTF8.GetString(result);
		}

		int HashToRandInt(string hash, int maxRangeInclusive) {
			if (maxRangeInclusive == 0) return 0;

			var bytes = Encoding.UTF8.GetBytes(hash).Take(30);

			int number = BitConverter.ToInt32(bytes.ToArray(), 0);

			int rand = (number % maxRangeInclusive) + 1;
			return rand;
		}

		string HashString(string strToHash) {
			byte[] sumBytes = Encoding.Unicode.GetBytes(strToHash);

			HashAlgorithm md5 = MD5.Create();
			byte[] result = md5.ComputeHash(sumBytes);

			return Encoding.UTF8.GetString(result);
		}

		class POIGlobalState : MonoBehaviour
		{
			private static POIGlobalState _instance;
			public static POIGlobalState Instance { get { return _instance; } }

			private void Awake()
			{
				if (_instance != null && _instance != this)
				{
					Destroy(this.gameObject);
				} else {
					_instance = this;
				}
			}

			[Serializable] public class POIStateDictionary : SerializableDictionary<string, POIState> {}
			[ReadOnly] public POIStateDictionary poiStates = new POIStateDictionary();

			/// <summary>
			/// Idempotent state setter
			/// </summary>
			public void AddState(POIState poiState) {
				if (!poiStates.ContainsKey(poiState.transformHash)) poiStates.Add(poiState.transformHash, poiState);
			}

			public POIState GetState(string transformHash) {
				POIState poiState;
				if (poiStates.TryGetValue(transformHash, out poiState)) return poiState;
				else return null;
			}
		}
	}
}
