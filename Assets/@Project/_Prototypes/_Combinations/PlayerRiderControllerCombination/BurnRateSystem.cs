using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.UpkeeperSystem;
using DuneRiders.RiderAICombination;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class BurnRateSystem : MonoBehaviour
	{
		public enum ResourceType {
			Fuel,
			ScrapMetal,
			PreciousMetal,
		}

		[SerializeField] UpkeepTracker upkeepTracker;
		[SerializeField] PlayerRepairSystem playerRepairSystem;

		UpkeepTracker.UpkeepType GetUpkeepEquivalentEnum(ResourceType resourceType) {
			switch (resourceType) {
				case ResourceType.Fuel:
					return UpkeepTracker.UpkeepType.Fuel;
				case ResourceType.ScrapMetal:
					return UpkeepTracker.UpkeepType.ScrapMetal;
				case ResourceType.PreciousMetal:
					return UpkeepTracker.UpkeepType.PreciousMetal;
				default:
					throw new NotImplementedException();
			}
		}

		public float GetResourceBurnRate(ResourceType resourceType) {
			return upkeepTracker.GetUpkeepRate(GetUpkeepEquivalentEnum(resourceType));
		}

		public float GetOneTimeResourceCost(ResourceType resourceType) {
			return 0f;
		}
	}
}
