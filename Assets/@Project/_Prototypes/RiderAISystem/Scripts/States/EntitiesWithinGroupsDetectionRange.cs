using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	public class EntitiesWithinGroupsDetectionRange : MonoBehaviour
	{
		EntitiesWithinGroupsDetectionRangeGlobalState globalState;
		public bool areAnyEnemyEntitiesWithinDetectionRange { get => globalState != null ? globalState.areAnyEnemyEntitiesWithinDetectionRange : false; }
		public bool areAnyEnemyEntitiesWithinThreatRange { get => globalState != null ? globalState.areAnyEnemyEntitiesWithinThreatRange : false; }

		void Awake() {
			InitializeGlobalState();
		}

		void InitializeGlobalState() {
			EntitiesWithinGroupsDetectionRangeGlobalState existingGlobalState = FindObjectOfType<EntitiesWithinGroupsDetectionRangeGlobalState>();
			if (existingGlobalState != null) {
				globalState = existingGlobalState;
				return;
			}

			globalState = new GameObject("EntitiesWithinGroupsDetectionRangeGlobalState").AddComponent<EntitiesWithinGroupsDetectionRangeGlobalState>();
		}

		[RequireComponent(typeof(AveragePositionOfRidersState))]
		[RequireComponent(typeof(AllActiveRidersState))]
		[RequireComponent(typeof(AllActiveTurretsState))]
		class EntitiesWithinGroupsDetectionRangeGlobalState : MonoBehaviour
		{
			private static EntitiesWithinGroupsDetectionRangeGlobalState _instance;
			public static EntitiesWithinGroupsDetectionRangeGlobalState Instance { get { return _instance; } }

			AveragePositionOfRidersState averagePositionOfRidersState;
			AllActiveRidersState allActiveRidersState;
			AllActiveTurretsState allActiveTurretsState;

			private void Awake()
			{
				if (_instance != null && _instance != this)
				{
					Destroy(this.gameObject);
				} else {
					_instance = this;
				}

				averagePositionOfRidersState = GetComponent<AveragePositionOfRidersState>();
				allActiveRidersState = GetComponent<AllActiveRidersState>();
				allActiveTurretsState = GetComponent<AllActiveTurretsState>();

				StartCoroutine(UpdateState());
			}

			Vector3 averageGroupPosition;
			float detectionRange = 550f;
			float threatRange = 250f;
			public bool areAnyEnemyEntitiesWithinDetectionRange { get; private set; }
			public bool areAnyEnemyEntitiesWithinThreatRange { get; private set; }

			IEnumerator UpdateState() {
				while (true) {
					averageGroupPosition = averagePositionOfRidersState.GetAverageWorldPositionOfRiders(Allegiance.Player);
					areAnyEnemyEntitiesWithinDetectionRange = AreAnyEnemyEntitiesWithinDistance(detectionRange);
					areAnyEnemyEntitiesWithinThreatRange = AreAnyEnemyEntitiesWithinDistance(threatRange);
					yield return new WaitForSeconds(1.5f);
				}
			}

			bool AreAnyEnemyEntitiesWithinDistance(float distance) {
				var positionOfClosestEnemyRider = PositionOfClosestEnemyRider();
				var positionOfClosestEnemyTurret = PositionOfClosestEnemyTurret();

				if (positionOfClosestEnemyRider != null && Vector3.Distance(averageGroupPosition, (Vector3) positionOfClosestEnemyRider) < distance) return true;
				if (positionOfClosestEnemyTurret != null && Vector3.Distance(averageGroupPosition, (Vector3) positionOfClosestEnemyTurret) < distance) return true;

				return false;
			}

			Vector3? PositionOfClosestEnemyRider() {
				var allRiders = allActiveRidersState.GetAllRidersOfAllegiance(Allegiance.Bandits);
				if (allRiders.Count == 0) return null;

				return allActiveRidersState.GetClosestRiderFromList(allRiders, averageGroupPosition).transform.position;
			}

			Vector3? PositionOfClosestEnemyTurret() {
				var allTurrets = allActiveTurretsState.GetAllTurretsOfAllegiance(Allegiance.Bandits);
				if (allTurrets.Count == 0) return null;

				return allActiveTurretsState.GetClosestTurretFromList(allTurrets, averageGroupPosition).transform.position;
			}

			#if UNITY_EDITOR
			void OnDrawGizmos() {
				DrawRange(Color.cyan, "DetectionRange", detectionRange);
				DrawRange(Color.red, "ThreatRange", threatRange);
			}

			void DrawRange(Color color, string label, float rangeDistance) {
				GUIStyle style = new GUIStyle();

				var labelPosition = averageGroupPosition;

				Handles.color = color;
				style.normal.textColor = color;
				labelPosition.y += 1f;
				Handles.Label(labelPosition, label, style);
				Handles.DrawWireDisc(averageGroupPosition, new Vector3(0, 1, 0), rangeDistance);
			}
			#endif
		}
	}
}
