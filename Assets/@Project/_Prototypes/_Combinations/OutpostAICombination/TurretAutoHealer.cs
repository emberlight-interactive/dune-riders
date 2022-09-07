using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.OutpostAI.Traits;
using DuneRiders.OutpostAI.State;

namespace DuneRiders.OutpostAICombination {
	[RequireComponent(typeof(RidersInRange))]
	public class TurretAutoHealer : MonoBehaviour
	{
		RidersInRange ridersInRange;
		(OutpostTurret turret, int maxHealth)[] turrets;

		[SerializeField] Allegiance enemyAllegiance = Allegiance.Player;

		[SerializeField] int healAmount = 5;
		[SerializeField] float healInterval = 4.0f;

		bool finishedStart = false;

		void Awake()
		{
			turrets = CompileTurretInfo();
			ridersInRange = GetComponent<RidersInRange>();
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
					for (int i = 0; i < turrets.Length; i++) {
						var currentTurret = turrets[i].turret;
						var turretHealthState = turrets[i].turret.GetComponent<HealthState>();

						if (turretHealthState.health <= 0) {
							turretHealthState.health = 1;
							currentTurret.gameObject.SetActive(true);
						} else if (turretHealthState.health < turrets[i].maxHealth) {
							var remainingHealthToReplenish = turrets[i].maxHealth - turretHealthState.health;
							if (remainingHealthToReplenish < healAmount) turretHealthState.health += remainingHealthToReplenish;
							else turretHealthState.health += healAmount;
						}
					}
				}

				yield return new WaitForSeconds(healInterval);
			}
		}

		(OutpostTurret turret, int maxHealth)[] CompileTurretInfo() {
			var compiledList = new List<(OutpostTurret turret, int maxHealth)>();
			var turrets = GetComponentsInChildren<OutpostTurret>();
			for (int i = 0; i < turrets.Length; i++) {
				compiledList.Add((turrets[i], turrets[i].GetComponent<HealthState>().MaxHealth));
			}

			return compiledList.ToArray();
		}

		bool AreAnyEnemyRidersInRange() {
			return ridersInRange.AreAnyRidersInRange(enemyAllegiance);
		}
	}
}
