using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DuneRiders.AI;
using DuneRiders.RiderAI.State;

namespace DuneRiders.OutpostAICombination {
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(AllActiveTurretsState))]
	public class RidersInRange : MonoBehaviour
	{
		AllActiveRidersState allActiveRidersState;
		AllActiveTurretsState allActiveTurretsState;

		[SerializeField] string rangeType = "Sight Range";
		[SerializeField] Color debugColor = Color.blue;
		[SerializeField] float labelYOffset = 1f;
		[SerializeField] float rangeDistance = 400f;

		void Awake()
		{
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			allActiveTurretsState = GetComponent<AllActiveTurretsState>();
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

		public bool AreAnyRidersInRange(Allegiance enemyAllegiance) {
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
