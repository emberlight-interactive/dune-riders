using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(AllActiveRidersState))]
	public class AveragePositionOfRidersState : MonoBehaviour
	{
		AllActiveRidersState allActiveRidersState;

		void Awake()
		{
			allActiveRidersState = GetComponent<AllActiveRidersState>();
		}

		public Vector3 GetAverageWorldPositionOfRiders(Rider.Allegiance? allegiance) {
			List<AllActiveRidersState.RiderData> targetRiderGroup;
			if (allegiance == null) {
				targetRiderGroup = allActiveRidersState.riderDataList;
			} else {
			 	targetRiderGroup = allActiveRidersState.GetAllRidersOfAllegiance((Rider.Allegiance) allegiance);
			}

			Vector3[] allRiderPositions = new Vector3[targetRiderGroup.Count];

			for (int i = 0; i < allRiderPositions.Length; i++) {
				allRiderPositions[i] = targetRiderGroup[i].transform.position;
			}

			return GetMeanVector(allRiderPositions);
		}

		Vector3 GetMeanVector(Vector3[] positions)
		{
			if (positions.Length == 0) {
				return Vector3.zero;
			}

			float x = 0f;
			float y = 0f;
			float z = 0f;

			foreach (Vector3 pos in positions)
			{
				x += pos.x;
				y += pos.y;
				z += pos.z;
			}

			return new Vector3(x / positions.Length, y / positions.Length, z / positions.Length);
		}
	}
}
