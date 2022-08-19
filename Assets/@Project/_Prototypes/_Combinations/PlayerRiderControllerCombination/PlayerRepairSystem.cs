using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GatheringSystem;
using DuneRiders.RiderAI.State;

namespace DuneRiders.PlayerRiderControllerCombination {
	[RequireComponent(typeof(EnemyAIEntitiesInRange))]
	public class PlayerRepairSystem : MonoBehaviour
	{
		EnemyAIEntitiesInRange enemiesInRange;
		[SerializeField] Gatherer gatherer;
		[SerializeField] DamageablePlayerRiderController damageablePlayer;

		[SerializeField] float repairCycleIntervalInSeconds = 4f;

		[SerializeField] int resourcesPerRepairCycle;
		[SerializeField] int healthPerRepairCycle;

		void Awake() {
			enemiesInRange = GetComponent<EnemyAIEntitiesInRange>();
		}

		void OnEnable() {
			StartCoroutine(RepairCycle());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		IEnumerator RepairCycle() {
			while (true) {
				if (IsRiderDamaged() && !AreAnyEnemiesAround() && AreAnyResourcesAvailable()) {
					MakeRepairTransaction();
				}

				yield return new WaitForSeconds(repairCycleIntervalInSeconds);
			}
		}

		void MakeRepairTransaction() {
			if (gatherer.GetManager(Gatherer.SupportedResources.ScrapMetal).Take(resourcesPerRepairCycle)) {
				damageablePlayer.Heal(healthPerRepairCycle);
			} else {
				var totalResourcesAvailable = gatherer.GetManager(Gatherer.SupportedResources.ScrapMetal).Amount();
				var percentageOfRequiredResourcesAvailable = (float) totalResourcesAvailable / (float) resourcesPerRepairCycle;
				healthPerRepairCycle = (int) (healthPerRepairCycle * percentageOfRequiredResourcesAvailable);

				if (gatherer.GetManager(Gatherer.SupportedResources.ScrapMetal).Take(totalResourcesAvailable)) {
					damageablePlayer.Heal(healthPerRepairCycle);
				}
			}
		}

		public int TotalResourcesRequiredToFullyRepair() {
			var numberOfRepairCyclesLeft = (float) HealthLeftToRepair() / (float) healthPerRepairCycle;
			return (int) ((float) resourcesPerRepairCycle * numberOfRepairCyclesLeft);
		}

		int HealthLeftToRepair() {
			return 100 - damageablePlayer.Health();
		}

		bool AreAnyEnemiesAround() {
			return enemiesInRange.EntityCount > 0;
		}

		bool AreAnyResourcesAvailable() {
			return gatherer.GetManager(Gatherer.SupportedResources.ScrapMetal).Amount() > 0;
		}

		bool IsRiderDamaged() {
			return damageablePlayer.Health() < 100;
		}
	}
}
