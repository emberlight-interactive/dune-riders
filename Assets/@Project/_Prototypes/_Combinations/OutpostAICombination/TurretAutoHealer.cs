using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DuneRiders.AI;
using DuneRiders.OutpostAI.Traits;
using DuneRiders.RiderAI.State;
using DuneRiders.OutpostAI.State;

namespace DuneRiders.OutpostAICombination {
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(AllActiveTurretsState))]
	public class TurretAutoHealer : MonoBehaviour
	{
		AllActiveRidersState allActiveRidersState;
		AllActiveTurretsState allActiveTurretsState;
		(OutpostTurret turret, int maxHealth)[] turrets;

		[SerializeField] string rangeType = "Sight Range";
		[SerializeField] Color debugColor = Color.blue;
		[SerializeField] float labelYOffset = 1f;
		[SerializeField] float rangeDistance = 400f;

		[SerializeField] Allegiance enemyAllegiance = Allegiance.Player;

		[SerializeField] int healAmount = 5;
		[SerializeField] float healInterval = 4.0f;

		void Awake()
		{
			turrets = CompileTurretInfo();
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			allActiveTurretsState = GetComponent<AllActiveTurretsState>();
		}

		void OnEnable() {
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
						var turretHealthState = turrets[i].turret.GetComponent<OutpostAI.State.HealthState>();

						if (turretHealthState.health <= 0) {
							currentTurret.gameObject.SetActive(true);
							turretHealthState.health = 1;
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
				compiledList.Add((turrets[i], turrets[i].GetComponent<OutpostAI.State.HealthState>().health));
			}

			return compiledList.ToArray();
		}

		#if UNITY_EDITOR
		void OnDrawGizmos() {
			GUIStyle style = new GUIStyle();

			var labelPosition = transform.position;

			Handles.color = debugColor;
        	style.normal.textColor = debugColor;
			labelPosition.y += labelYOffset;
			Handles.Label(labelPosition, rangeType, style);
        	Handles.DrawWireDisc(transform.position, new Vector3(0, 1, 0), rangeDistance);
		}
		#endif

		bool AreAnyEnemyRidersInRange() {
			var allEnemyRiders = allActiveRidersState.GetAllRidersOfAllegiance(enemyAllegiance);

			foreach (var rider in allEnemyRiders) {
				if (Vector3.Distance(transform.position, rider.transform.position) <= rangeDistance) {
					return true;
				}
			}

			return false;
		}

	}
}
