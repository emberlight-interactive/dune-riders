using System;
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
			public int lootableLocationIndex = 0;
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
		ProceduralTools proceduralTools;
		string transformHash;

		[SerializeField, ReadOnly] POIState state;

		void Awake() {
			proceduralTools = new ProceduralTools(transform);
			transformHash = proceduralTools.BuildTransformHash();
			InitializeState();
		}

		void OnEnable() {
			SpawnLootables();
			StartCoroutine(UpdateStateOfLootables());
		}

		void OnDisable() {
			StopAllCoroutines();
			DespawnLootables();
		}

		void FixedUpdate() {
			var str = proceduralTools.BuildTransformHash();
		}

		void InitializeState() {
			var poiState = new POIState() {
				transformHash = transformHash,
				lootableStates = CompileLootableStates(),
			};

			GlobalState.InitState<POIGlobalState, string, POIState>(transformHash, poiState, out state);
		}

		int GetNumberOfLootables() {
			int numberOfLootables;

			if (minimumNumberOfLootables >= lootLocations.Count) {
				numberOfLootables = lootLocations.Count;
			} else {
				var additionalLootables = proceduralTools.HashToRandInt(transformHash, lootLocations.Count - minimumNumberOfLootables);
				numberOfLootables = additionalLootables + minimumNumberOfLootables;
			}

			return numberOfLootables;
		}

		List<LootableState> CompileLootableStates() {
			var lootableStates = new List<LootableState>();
			var numberOfLootables = GetNumberOfLootables();

			if (numberOfLootables == 0 || lootables.Count == 0) return lootableStates;

			string currentHash = proceduralTools.HashString(transformHash);
			for (int i = 0; i < numberOfLootables; i++) {
				lootableStates.Add(new LootableState {
					lootable = SelectLootable(currentHash).gameObject,
					lootableLocationIndex = i,
				});

				currentHash = proceduralTools.HashString(currentHash);
			}

			return lootableStates;
		}

		Lootable SelectLootable(string seed) {
			var combinedAvailabilityValues = lootables.Sum((lootable) => lootable.availabilityValue);
			var randomNumber = proceduralTools.HashToRandInt(seed, combinedAvailabilityValues);

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

				lootableState.spawnedLootable = Instantiate(lootableState.lootable, lootLocations[lootableState.lootableLocationIndex]);
			}
		}

		void DespawnLootables() {
			foreach (var lootableState in state.lootableStates) {
				if (!lootableState.spawnedLootable) continue;

				Destroy(lootableState.spawnedLootable);
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

		class POIGlobalState : GlobalStateGameObject<string, POIState> {}
	}
}
