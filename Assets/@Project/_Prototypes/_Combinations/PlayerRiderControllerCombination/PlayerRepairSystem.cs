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
		[SerializeField] DamageablePlayerRiderController damageablePlayer;

		[SerializeField] float repairCycleIntervalInSeconds = 4f;
		[SerializeField] float healthPerRepairCycle;

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
				if (IsRiderDamaged() && !AreAnyEnemiesAround()) {
					RunRepairCycle();
				}

				yield return new WaitForSeconds(repairCycleIntervalInSeconds);
			}
		}

		void RunRepairCycle() {
			damageablePlayer.Heal(HealthLeftToRepair() < healthPerRepairCycle ? HealthLeftToRepair() : healthPerRepairCycle);
		}

		float HealthLeftToRepair() {
			return damageablePlayer.MaxHealth() - damageablePlayer.Health();
		}

		bool AreAnyEnemiesAround() {
			return enemiesInRange.EntityCount > 0;
		}

		bool IsRiderDamaged() {
			return damageablePlayer.Health() < damageablePlayer.MaxHealth();
		}
	}
}
