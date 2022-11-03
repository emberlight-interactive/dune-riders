using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.State;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(Rider))]
	[RequireComponent(typeof(EntitiesWithinGroupsDetectionRange))]
	[RequireComponent(typeof(HealthState))]
	public class RepairSystem : MonoBehaviour
	{
		Rider rider;
		EntitiesWithinGroupsDetectionRange entitiesWithinGroupsDetectionRange;
		HealthState healthState;

		bool _isCurrentlyRepairing = false;
		bool isCurrentlyRepairing {
			get => _isCurrentlyRepairing;
			set {
				ToggleRiderRepairIcon(value);
				_isCurrentlyRepairing = value;
			}
		}
		public bool IsCurrentlyRepairing { get => isCurrentlyRepairing; }

		[SerializeField] float repairCycleIntervalInSeconds = 4f;

		[SerializeField] float healthPerHeavyChasisRepairCycle;
		[SerializeField] float healthPerNormalChasisRepairCycle;
		[SerializeField] float healthPerLightChasisRepairCycle;

		[SerializeField] GameObject riderRepairIcon;

		void Awake() {
			rider = GetComponent<Rider>();
			entitiesWithinGroupsDetectionRange = GetComponent<EntitiesWithinGroupsDetectionRange>();
			healthState = GetComponent<HealthState>();
			ToggleRiderRepairIcon(false);
		}

		void OnEnable() {
			StartCoroutine(RepairCycle());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		IEnumerator RepairCycle() {
			while (true) {
				if (!AreAnyEnemiesInDetectionRange() && IsRiderDamaged() && IsRiderInParty()) {
					isCurrentlyRepairing = true;
					ProcessChasisRepairCycle(rider.chasisType);
				} else {
					isCurrentlyRepairing = false;
				}

				yield return new WaitForSeconds(repairCycleIntervalInSeconds);
			}
		}

		void ProcessChasisRepairCycle(Rider.ChasisType chasis) {
			var healthToReplenish = GetHealthPerRepairCycle(chasis);

			var healthLeftToRepair = HealthLeftToRepair();

			if (healthLeftToRepair < healthToReplenish) {
				healthToReplenish = healthLeftToRepair;
			}

			healthState.health += healthToReplenish;
		}

		float GetHealthPerRepairCycle(Rider.ChasisType chasis) {
			switch (rider.chasisType) {
				case Rider.ChasisType.Heavy:
					return healthPerHeavyChasisRepairCycle;
				case Rider.ChasisType.Normal:
					return healthPerNormalChasisRepairCycle;
				case Rider.ChasisType.Light:
					return healthPerLightChasisRepairCycle;
				default:
					return healthPerLightChasisRepairCycle;
			}
		}

		void ToggleRiderRepairIcon(bool showIcon) {
			riderRepairIcon.gameObject.SetActive(showIcon);
		}

		float HealthLeftToRepair() {
			return healthState.MaxHealth - healthState.health;
		}

		bool IsRiderDamaged() {
			return healthState.health < healthState.MaxHealth;
		}

		bool IsRiderInParty() {
			return rider.allegiance == Allegiance.Player;
		}

		bool AreAnyEnemiesInDetectionRange() {
			return entitiesWithinGroupsDetectionRange.areAnyEnemyEntitiesWithinDetectionRange;
		}
	}
}
