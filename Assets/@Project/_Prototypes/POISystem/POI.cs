using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.POISystem {
	public class POI : MonoBehaviour
	{
		[Serializable]
		public class POIState {
			public string transformHash;
			public List<LootableState> lootableStates;
		}

		[Serializable]
		public class LootableState {
			public bool harvested = false;
			public int lootableLocationIndex = 0;
			public string lootable;
		}

		[Serializable]
		public class Lootable {
			public GameObject gameObject;
			public int availabilityValue = 1;
		}

		[Serializable] public class LootablesDictionary : SerializableDictionary<string, Lootable> {}
		[SerializeField] LootablesDictionary lootables = new LootablesDictionary();
		[SerializeField] List<Transform> lootLocations = new List<Transform>();
		[SerializeField, ReadOnly] Dictionary<int, GameObject> spawnedLootables = new Dictionary<int, GameObject>();
		[SerializeField] int minimumNumberOfLootables = 3;

		[SerializeField] UnityEvent poiTouched;
		[SerializeField] UnityEvent poiExhausted;

		ProceduralTools proceduralTools;
		string transformHash;

		[SerializeField, ReadOnly] POIState state;

		void Awake() {
			proceduralTools = new ProceduralTools(transform, true);
			transformHash = proceduralTools.BuildTransformHash();
			InitializeState();
		}

		void OnEnable() {
			SpawnLootables();
			StartCoroutine(UpdateStateOfLootables());
			StartCoroutine(WatchPOITouchedEventTrigger());
			StartCoroutine(WatchPOIExhaustedEventTrigger());
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

			GlobalState.InitState<POIGlobalState, string, POIState>(transformHash, poiState, out state, new Type[] { typeof(LoadLocalComponentsOnAwake) });
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
					lootable = SelectLootable(currentHash),
					lootableLocationIndex = i,
				});

				currentHash = proceduralTools.HashString(currentHash);
			}

			return lootableStates;
		}

		string SelectLootable(string seed) {
			var combinedAvailabilityValues = lootables.Sum((lootable) => lootable.Value.availabilityValue);
			var randomNumber = proceduralTools.HashToRandInt(seed, combinedAvailabilityValues);

			int currentIteratedRange = 0;
			foreach (var lootable in lootables) {
				currentIteratedRange += lootable.Value.availabilityValue;

				if (randomNumber <= currentIteratedRange) return lootable.Key;
			}

			return lootables.First().Key;
		}

		void SpawnLootables() {
			foreach (var lootableState in state.lootableStates) {
				if (lootableState.harvested) continue;

				spawnedLootables[lootableState.lootableLocationIndex] = Instantiate(lootables[lootableState.lootable].gameObject, lootLocations[lootableState.lootableLocationIndex]);
			}
		}

		void DespawnLootables() {
			foreach (var spawnedLootable in spawnedLootables) {
				if (spawnedLootable.Value == null) continue;

				Destroy(spawnedLootable.Value);
			}
		}

		IEnumerator UpdateStateOfLootables() {
			while (true) {
				UpdateHarvestedStateOfLootables();
				yield return new WaitForSeconds(0.2f); // todo: Prevent "destroy" -> save and exit -> load -> destroy loop (very low priority)
			}
		}

		IEnumerator WatchPOITouchedEventTrigger() {
			while (true) {
				if (IsPOIPartiallyLooted()) {
					poiTouched?.Invoke();
					yield break;
				}
				yield return new WaitForSeconds(0.2f);
			}
		}

		IEnumerator WatchPOIExhaustedEventTrigger() {
			while (true) {
				if (IsPOIFullyLooted()) {
					poiExhausted?.Invoke();
					yield break;
				}
				yield return new WaitForSeconds(0.2f);
			}
		}

		bool IsPOIPartiallyLooted() {
			var hasALootableBeenHarvested = false;
			var hasALootableRemainedUntouched = false;
			foreach (var lootableState in state.lootableStates) {
				if (lootableState.harvested) hasALootableBeenHarvested = true;
				if (!lootableState.harvested) hasALootableRemainedUntouched = true;
			}

			return (hasALootableBeenHarvested && hasALootableRemainedUntouched);
		}

		bool IsPOIFullyLooted() {
			return state.lootableStates.Find((lootableState) => lootableState.harvested == false) == null;
		}

		void UpdateHarvestedStateOfLootables() {
			foreach (var lootableState in state.lootableStates) {
				if (
					spawnedLootables.ContainsKey(lootableState.lootableLocationIndex)
					&& (spawnedLootables[lootableState.lootableLocationIndex] == null
					|| spawnedLootables[lootableState.lootableLocationIndex].activeSelf == false)) {
					lootableState.harvested = true;
				}
			}
		}

		class POIGlobalState : GlobalStateGameObject<string, POIState> {}
	}
}
