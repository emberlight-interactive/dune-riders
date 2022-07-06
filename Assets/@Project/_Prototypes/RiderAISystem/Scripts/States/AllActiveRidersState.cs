using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.RiderAI.Traits;

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
		[ReadOnly] public int updateIntervalInSeconds = 4;

		void Awake() {
			InitializeGlobalState();
			riderDataList = globalState.riderDataList;
		}

		void InitializeGlobalState() {
			AllActiveRidersGlobalState existingGlobalState = FindObjectOfType<AllActiveRidersGlobalState>();
			if (existingGlobalState != null) {
				globalState = existingGlobalState;
				return;
			}

			globalState = new GameObject("AllActiveRidersGlobalState").AddComponent<AllActiveRidersGlobalState>();
			globalState.updateIntervalInSeconds = updateIntervalInSeconds;
		}

		public List<RiderData> GetAllRidersOfAllegiance(Rider.Allegiance allegiance) {
			List<RiderData> enemyRiderDataList = new List<RiderData>();
			foreach (var riderData in riderDataList) {
				if (riderData.rider.allegiance == allegiance) {
					enemyRiderDataList.Add(riderData);
				}
			}
			return enemyRiderDataList;
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

				StartCoroutine(UpdateRiderInformation());
			}

			[ReadOnly] public List<RiderData> riderDataList = new List<RiderData>();
			[ReadOnly] public int updateIntervalInSeconds;

			IEnumerator UpdateRiderInformation() {
				while (true) {
					var newListOfRiders = ScanAndReturnListofRiders();
					riderDataList.Clear();
					riderDataList.AddRange(newListOfRiders);

					yield return new WaitForSeconds(updateIntervalInSeconds);
				}
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
