using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.State;
using DuneRiders.GatheringSystem;
using DuneRiders.AI;
using DuneRiders.PlayerRiderControllerCombination;

namespace DuneRiders.RiderAICombination {
	[RequireComponent(typeof(Rider))]
	[RequireComponent(typeof(EntitiesWithinGroupsDetectionRange))]
	[RequireComponent(typeof(HealthState))]
	public class RepairSystem : MonoBehaviour
	{
		Rider rider;
		EntitiesWithinGroupsDetectionRange entitiesWithinGroupsDetectionRange;
		HealthState healthState;

		Gatherer gatherer;
		BurnRateSystem burnRateSystem;

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

		[SerializeField] int resourcesPerHeavyChasisRepairCycle;
		[SerializeField] int healthPerHeavyChasisRepairCycle;

		[SerializeField] int resourcesPerNormalChasisRepairCycle;
		[SerializeField] int healthPerNormalChasisRepairCycle;

		[SerializeField] int resourcesPerLightChasisRepairCycle;
		[SerializeField] int healthPerLightChasisRepairCycle;

		[SerializeField] GameObject riderRepairIcon;

		void Awake() {
			rider = GetComponent<Rider>();
			entitiesWithinGroupsDetectionRange = GetComponent<EntitiesWithinGroupsDetectionRange>();
			healthState = GetComponent<HealthState>();
			gatherer = FindObjectOfType<Gatherer>();
			burnRateSystem = FindObjectOfType<BurnRateSystem>();
			ToggleRiderRepairIcon(false);
		}

		void OnEnable() {
			burnRateSystem.AttachMyself(this);
			StartCoroutine(RepairCycle());
		}

		void OnDisable() {
			burnRateSystem.RemoveMyself(this);
			StopAllCoroutines();
		}

		IEnumerator RepairCycle() {
			while (true) {
				if (!AreAnyEnemiesInDetectionRange() && IsRiderDamaged() && IsRiderInParty() && AreAnyResourcesAvailable()) {
					isCurrentlyRepairing = true;
					ProcessChasisRepairCycle(rider.chasisType);
				} else {
					isCurrentlyRepairing = false;
				}

				yield return new WaitForSeconds(repairCycleIntervalInSeconds);
			}
		}

		public int TotalResourcesRequiredToFullyRepair() {
			var repairCycleInfo = GetRepairInformationFromChasis(rider.chasisType);
			var numberOfRepairCyclesLeft = (float) HealthLeftToRepair() / (float) repairCycleInfo.healthToReplenish;
			return (int) ((float) repairCycleInfo.resourcesToSpend * numberOfRepairCyclesLeft);
		}

		void ProcessChasisRepairCycle(Rider.ChasisType chasis) {
			var repairInfo = GetRepairInformationFromChasis(chasis);

			int resourcesToSpend = repairInfo.resourcesToSpend;
			int healthToReplenish = repairInfo.healthToReplenish;

			var healthLeftToRepair = HealthLeftToRepair();

			if (healthLeftToRepair < healthToReplenish) {
				var percentageOfReplenishedHealth = (float) healthLeftToRepair / (float) healthToReplenish;
				resourcesToSpend = (int) (resourcesToSpend * percentageOfReplenishedHealth);
				healthToReplenish = healthLeftToRepair;
			}

			MakeRepairTransaction(resourcesToSpend, healthToReplenish);
		}

		void MakeRepairTransaction(int resourcesToSpend, int healthToReplenish) {
			if (gatherer.GetManager(Gatherer.SupportedResources.ScrapMetal).Take(resourcesToSpend)) {
				healthState.health += healthToReplenish;
			} else {
				var totalResourcesAvailable = gatherer.GetManager(Gatherer.SupportedResources.ScrapMetal).Amount();
				var percentageOfRequiredResourcesAvailable = (float) totalResourcesAvailable / (float) resourcesToSpend;
				healthToReplenish = (int) (healthToReplenish * percentageOfRequiredResourcesAvailable);

				if (gatherer.GetManager(Gatherer.SupportedResources.ScrapMetal).Take(totalResourcesAvailable)) {
					healthState.health += healthToReplenish;
				}
			}
		}

		(int resourcesToSpend, int healthToReplenish) GetRepairInformationFromChasis(Rider.ChasisType chasis) {
			switch (rider.chasisType) {
				case Rider.ChasisType.Heavy:
					return (resourcesPerHeavyChasisRepairCycle, healthPerHeavyChasisRepairCycle);
				case Rider.ChasisType.Normal:
					return (resourcesPerNormalChasisRepairCycle, healthPerNormalChasisRepairCycle);
				case Rider.ChasisType.Light:
					return (resourcesPerLightChasisRepairCycle, healthPerLightChasisRepairCycle);
				default:
					return (resourcesPerLightChasisRepairCycle, healthPerLightChasisRepairCycle);
			}
		}

		void ToggleRiderRepairIcon(bool showIcon) {
			riderRepairIcon.gameObject.SetActive(showIcon);
		}

		int HealthLeftToRepair() {
			return 100 - healthState.health;
		}

		bool IsRiderDamaged() {
			return healthState.health < 100;
		}

		bool IsRiderInParty() {
			return rider.allegiance == Allegiance.Player;
		}

		bool AreAnyResourcesAvailable() {
			return gatherer.GetManager(Gatherer.SupportedResources.ScrapMetal).Amount() > 0;
		}

		bool AreAnyEnemiesInDetectionRange() {
			return entitiesWithinGroupsDetectionRange.areAnyEnemyEntitiesWithinDetectionRange;
		}
	}
}
