using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.OutpostAI.Traits;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	public class AllNearbyOutpostsState : MonoBehaviour
	{
		AllNearbyOutpostsGlobalState globalState;
		[ReadOnly] public List<Outpost> outposts = new List<Outpost>();
		[ReadOnly] public int updateIntervalInSeconds = 4;

		void Awake() {
			InitializeGlobalState();
			outposts = globalState.outposts;
		}

		void InitializeGlobalState() {
			AllNearbyOutpostsGlobalState existingGlobalState = FindObjectOfType<AllNearbyOutpostsGlobalState>();
			if (existingGlobalState != null) {
				globalState = existingGlobalState;
				return;
			}

			globalState = new GameObject("AllNearbyOutpostsGlobalState").AddComponent<AllNearbyOutpostsGlobalState>();
			globalState.updateIntervalInSeconds = updateIntervalInSeconds;
		}

		public List<Outpost> GetAllOutpostsOfAllegiance(Allegiance allegiance) {
			List<Outpost> targetOutpostList = new List<Outpost>();
			foreach (var turret in outposts) {
				if (turret.allegiance == allegiance) {
					targetOutpostList.Add(turret);
				}
			}
			return targetOutpostList;
		}

		public Outpost GetClosestOutpostFromList(List<Outpost> turrets = null) {
			return (turrets ?? outposts)
				.OrderBy(t=>(t.transform.position - transform.position).sqrMagnitude)
				.First();
		}

		class AllNearbyOutpostsGlobalState : MonoBehaviour
		{
			private static AllNearbyOutpostsGlobalState _instance;
			public static AllNearbyOutpostsGlobalState Instance { get { return _instance; } }

			private void Awake()
			{
				if (_instance != null && _instance != this)
				{
					Destroy(this.gameObject);
				} else {
					_instance = this;
				}

				StartCoroutine(UpdateOutpostsInformation());
			}

			[ReadOnly] public List<Outpost> outposts = new List<Outpost>();
			[ReadOnly] public int updateIntervalInSeconds;

			IEnumerator UpdateOutpostsInformation() {
				while (true) {
					outposts.Clear();
					outposts.AddRange(FindObjectsOfType<Outpost>());

					yield return new WaitForSeconds(updateIntervalInSeconds);
				}
			}
		}
	}
}
