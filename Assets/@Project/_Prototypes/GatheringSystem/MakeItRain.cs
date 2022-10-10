using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GatheringSystem {
	public class MakeItRain : MonoBehaviour
	{
		[SerializeField] Transform[] gatherableSpawnLocations;
		[SerializeField] Gatherable[] gatherablesToSpawn;
		public float increaseRareProbabilityMultipler = 0;

		public void SpawnLootables() {
			for (int i = 0; i < gatherableSpawnLocations.Length; i++) {
				var gatherable = SelectGatherable();
				SimplePool.Spawn(gatherable.gameObject, gatherableSpawnLocations[i].position, gatherable.transform.rotation);
			}
		}

		Gatherable SelectGatherable() {
			var combinedRarityValues = gatherablesToSpawn.Sum((gatherable) => gatherable.rarityValue);

			var gatherablePercentageMultiplers = GetGatherablePercentageMultiplers(gatherablesToSpawn, combinedRarityValues);
			var gatherableWithNewRarityMultiplers = GetNewGatherableRarityValues(gatherablePercentageMultiplers);

			var newCombinedRarityValues = gatherableWithNewRarityMultiplers.Sum((gatherableWithNewRarityMultipler) => gatherableWithNewRarityMultipler.newRarityValue);

			var randomNumber = Random.Range(0f, newCombinedRarityValues);
			float currentIteratedRange = 0;
			foreach (var gatherableWithNewRarityMultipler in gatherableWithNewRarityMultiplers) {
				currentIteratedRange += gatherableWithNewRarityMultipler.newRarityValue;

				if (randomNumber <= currentIteratedRange) return gatherableWithNewRarityMultipler.Item1;
			}

			return gatherablesToSpawn[0];
		}

		List<(Gatherable, float percentageMultipler)> GetGatherablePercentageMultiplers(Gatherable[] gatherables, float combinedRarityValues) {
			var gatherablePercentageMultiplers = new List<(Gatherable, float percentageMultipler)>();

			foreach (var gatherable in gatherables) {
				gatherablePercentageMultiplers.Add((gatherable, (1f - (gatherable.rarityValue / combinedRarityValues)) * increaseRareProbabilityMultipler));
			}

			return gatherablePercentageMultiplers;
		}

		List<(Gatherable, float newRarityValue)> GetNewGatherableRarityValues(List<(Gatherable, float percentageMultipler)> gatherablePercentageMultiplers) {
			var gatherableNewRarityValues = new List<(Gatherable, float newRarityValue)>();

			foreach (var gatherableAndPercentageMultipler in gatherablePercentageMultiplers) {
				gatherableNewRarityValues.Add((gatherableAndPercentageMultipler.Item1, gatherableAndPercentageMultipler.Item1.rarityValue + (gatherableAndPercentageMultipler.Item1.rarityValue * gatherableAndPercentageMultipler.percentageMultipler)));
			}

			return gatherableNewRarityValues;
		}
	}
}
