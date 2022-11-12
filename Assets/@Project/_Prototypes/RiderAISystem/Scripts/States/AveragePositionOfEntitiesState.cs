using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(AllActiveTurretsState))]
	[RequireComponent(typeof(AllNearbyOutpostsState))]
	public class AveragePositionOfEntitiesState : MonoBehaviour
	{
		AllActiveRidersState allActiveRidersState;
		AllActiveTurretsState allActiveTurretsState;
		AllNearbyOutpostsState allNearbyOutpostsState;

		List<Transform> entityTransforms = new List<Transform>();

		void Awake()
		{
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			allActiveTurretsState = GetComponent<AllActiveTurretsState>();
			allNearbyOutpostsState = GetComponent<AllNearbyOutpostsState>();
		}

		public Vector3 GetAverageWorldPositionOfEntities(Allegiance? allegiance) {
			entityTransforms.Clear();
			entityTransforms.AddRange(GetAllRiderTransforms(allegiance));
			entityTransforms.AddRange(GetAllTurretTransforms(allegiance));
			entityTransforms.AddRange(GetAllOutpostTransforms(allegiance));

			return GetMeanPositionVector(entityTransforms);
		}

		IEnumerable<Transform> GetAllRiderTransforms(Allegiance? allegiance) {
			if (allegiance == null) {
				return allActiveRidersState.riderDataList.Select((riderData) => riderData.transform);
			} else {
				return allActiveRidersState.GetAllRidersOfAllegiance((Allegiance) allegiance).Select((riderData) => riderData.transform);
			}
		}

		IEnumerable<Transform> GetAllTurretTransforms(Allegiance? allegiance) {
			if (allegiance == null) {
				return allActiveTurretsState.outpostTurrets.Select((turret) => turret.transform);
			} else {
				return allActiveTurretsState.GetAllTurretsOfAllegiance((Allegiance) allegiance).Select((turret) => turret.transform);
			}
		}

		IEnumerable<Transform> GetAllOutpostTransforms(Allegiance? allegiance) {
			if (allegiance == null) {
				return allNearbyOutpostsState.outposts.Select((outpost) => outpost.transform);
			} else {
				return allNearbyOutpostsState.GetAllOutpostsOfAllegiance((Allegiance) allegiance).Select((outpost) => outpost.transform);
			}
		}

		Vector3 GetMeanPositionVector(List<Transform> transforms)
		{
			if (transforms.Count == 0) {
				return Vector3.zero;
			}

			float x = 0f;
			float y = 0f;
			float z = 0f;

			foreach (Transform trans in transforms)
			{
				x += trans.position.x;
				y += trans.position.y;
				z += trans.position.z;
			}

			return new Vector3(x / transforms.Count, y / transforms.Count, z / transforms.Count);
		}
	}
}
