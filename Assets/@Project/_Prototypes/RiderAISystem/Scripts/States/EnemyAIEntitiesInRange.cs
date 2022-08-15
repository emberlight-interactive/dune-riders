using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DuneRiders.RiderAI.Traits;
using Sirenix.OdinInspector;

namespace DuneRiders.RiderAI.State {
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(AllActiveTurretsState))]
	[RequireComponent(typeof(Rider))]
	public class EnemyAIEntitiesInRange : MonoBehaviour
	{
		public enum EntityType {
			Turret,
			Rider,
		}

		AllActiveRidersState allActiveRidersState;
		AllActiveTurretsState allActiveTurretsState;
		Rider rider;

		[SerializeField] string rangeType = "Sight Range";
		[SerializeField] Color debugColor = Color.blue;
		[SerializeField] float labelYOffset = 1f;
		[SerializeField] float rangeDistance = 400f;

		List<(EntityType, Transform)> entitiesInSight = new List<(EntityType, Transform)>();
		[SerializeField, ReadOnly] public List<(EntityType, Transform)> EntitiesInSight { get => entitiesInSight; }
		[SerializeField, ReadOnly] int entityCount = 0;
		public int EntityCount { get => entityCount; }

		void Awake()
		{
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			allActiveTurretsState = GetComponent<AllActiveTurretsState>();
			rider = GetComponent<Rider>();
		}

		void OnEnable() {
			StartCoroutine(UpdateState());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		IEnumerator UpdateState() {
			while (true) {
				var allEnemyRidersInSight = GetAllEnemyRidersInSight();
				var allEnemyTurretsInSight = GetAllEnemyTurretsInSight();
				entitiesInSight.Clear();
				entitiesInSight.AddRange(allEnemyRidersInSight);
				entitiesInSight.AddRange(allEnemyTurretsInSight);
				entityCount = entitiesInSight.Count;

				yield return new WaitForSeconds(2f);
			}
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

		List<(EntityType, Transform)> GetAllEnemyRidersInSight() {
			var enemyRidersInSight = new List<(EntityType, Transform)>();
			var allEnemyRiders = allActiveRidersState.GetAllRidersOfAllegiance(rider.enemyAllegiance);

			foreach (var rider in allEnemyRiders) {
				if (Vector3.Distance(transform.position, rider.transform.position) <= rangeDistance) {
					enemyRidersInSight.Add((EntityType.Rider, rider.transform));
				}
			}

			return enemyRidersInSight;
		}

		List<(EntityType, Transform)> GetAllEnemyTurretsInSight() {
			var enemyTurretsInSight = new List<(EntityType, Transform)>();
			var allEnemyTurrets = allActiveTurretsState.GetAllTurretsOfAllegiance(rider.enemyAllegiance);

			foreach (var turret in allEnemyTurrets) {
				if (Vector3.Distance(transform.position, turret.transform.position) <= rangeDistance) {
					enemyTurretsInSight.Add((EntityType.Turret, turret.transform));
				}
			}

			return enemyTurretsInSight;
		}
	}
}
