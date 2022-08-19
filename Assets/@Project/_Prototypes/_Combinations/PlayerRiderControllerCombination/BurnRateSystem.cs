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
		List<RepairSystem> repairSystems = new List<RepairSystem>();

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
			if (resourceType == ResourceType.ScrapMetal) return GetTotalScrapForRepairs();
			return 0f;
		}

		float GetTotalScrapForRepairs() {
			float totalScrapToSpend = 0f;

			foreach (var repairer in repairSystems) {
				totalScrapToSpend += repairer.TotalResourcesRequiredToFullyRepair();
			}

			totalScrapToSpend += playerRepairSystem.TotalResourcesRequiredToFullyRepair();

			return totalScrapToSpend;
		}

		public void AttachMyself(RepairSystem repairSystem) {
			if (!repairSystems.Exists((v) => v == repairSystem)) {
				repairSystems.Add(repairSystem);
			}
		}

		public void RemoveMyself(RepairSystem repairSystem) {
			if (repairSystems.Exists((v) => v == repairSystem)) {
				repairSystems.Remove(repairSystem);
			}
		}
	}
}
