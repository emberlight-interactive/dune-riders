using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.RiderAI.Traits;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	public class AllActiveRidersState : MonoBehaviour
	{
		[System.Serializable]
		public struct RiderData {
			public Transform transform;
			public Rider rider;
		}

		AllActiveRidersGlobalState globalState;
		[ReadOnly] public List<RiderData> riderDataList = new List<RiderData>();
		[ReadOnly] public int updateIntervalInSeconds = 4; // todo: Going to give performance a hard time if one interval is set at 1 seconds and initializes the global state first

		void Awake() {
			InitializeGlobalState();
			riderDataList = globalState.riderDataList;
		}

		void InitializeGlobalState() {
			AllActiveRidersGlobalState existingGlobalState = FindObjectOfType<AllActiveRidersGlobalState>();
			if (existingGlobalState != null) {
				globalState = existingGlobalState;
				globalState.ForceStateUpdate();
				return;
			}

			globalState = new GameObject("AllActiveRidersGlobalState").AddComponent<AllActiveRidersGlobalState>();
			globalState.updateIntervalInSeconds = updateIntervalInSeconds;
		}

		public List<RiderData> GetAllRidersOfAllegiance(Allegiance allegiance) {
			List<RiderData> targetRiderDataList = new List<RiderData>();
			foreach (var riderData in riderDataList) {
				if (riderData.rider.allegiance == allegiance) {
					targetRiderDataList.Add(riderData);
				}
			}
			return targetRiderDataList;
		}

		public RiderData GetClosestRiderFromList(List<RiderData> riders) {
			return riders
				.OrderBy(t=>(t.transform.position - transform.position).sqrMagnitude)
				.First();
		}

		class AllActiveRidersGlobalState : MonoBehaviour
		{
			private static AllActiveRidersGlobalState _instance;
			public static AllActiveRidersGlobalState Instance { get { return _instance; } }

			private void Awake()
			{
				if (_instance != null && _instance != this)
				{
					Destroy(this.gameObject);
				} else {
					_instance = this;
				}

				StartCoroutine(UpdateState());
			}

			[ReadOnly] public List<RiderData> riderDataList = new List<RiderData>();
			[ReadOnly] public int updateIntervalInSeconds;

			public void ForceStateUpdate() {
				UpdateRiderInformation();
			}

			IEnumerator UpdateState() {
				while (true) {
					UpdateRiderInformation();
					yield return new WaitForSeconds(updateIntervalInSeconds);
				}
			}

			void UpdateRiderInformation() {
				var newListOfRiders = ScanAndReturnListofRiders();
				riderDataList.Clear();
				riderDataList.AddRange(newListOfRiders);
			}

			List<RiderData> ScanAndReturnListofRiders() {
				Rider[] riders = FindObjectsOfType<Rider>();
				List<RiderData> newListOfRiders = new List<RiderData>();
				foreach (var rider in riders) {
					newListOfRiders.Add(new RiderData() {
						transform = rider.transform,
						rider = rider,
					});
				}

				return newListOfRiders;
			}
		}
	}
}
