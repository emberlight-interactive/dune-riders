using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.OutpostAI;
using DuneRiders.OutpostAI.Traits;

namespace DuneRiders.OutpostAICombination {
	[RequireComponent(typeof(StructureHealth))]
	[RequireComponent(typeof(RidersInRange))]
	public class StructureAutoHealer : MonoBehaviour
	{
		[SerializeField] Allegiance enemyAllegiance = Allegiance.Player;

		[SerializeField] float healAmount = 5;
		[SerializeField] float healInterval = 4.0f;

		RidersInRange ridersInRange;
		StructureHealth structureHealth;

		bool finishedStart = false;

		void Awake() {
			ridersInRange = GetComponent<RidersInRange>();
			structureHealth = GetComponent<StructureHealth>();
		}

		void OnEnable() {
			if (finishedStart) StartCoroutine(AutoHealLoop());
		}

		void Start() {
			finishedStart = true;
			StartCoroutine(AutoHealLoop());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		IEnumerator AutoHealLoop() {
			while (true) {
				if (!AreAnyEnemyRidersInRange()) {
					var remainingHealthToReplenish = structureHealth.MaxHealth - structureHealth.health;
					if (remainingHealthToReplenish < healAmount) structureHealth.health += remainingHealthToReplenish;
					else structureHealth.health += healAmount;
				}

				yield return new WaitForSeconds(healInterval);
			}
		}

		bool AreAnyEnemyRidersInRange() {
			return ridersInRange.AreAnyRidersInRange(enemyAllegiance);
		}
	}
}
