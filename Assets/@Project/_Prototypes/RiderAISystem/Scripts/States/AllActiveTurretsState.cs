using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.OutpostAI.Traits;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	public class AllActiveTurretsState : MonoBehaviour
	{
		AllActiveTurretsGlobalState globalState;
		[ReadOnly] public List<OutpostTurret> outpostTurrets = new List<OutpostTurret>();
		[ReadOnly] public int updateIntervalInSeconds = 4;

		void Awake() {
			InitializeGlobalState();
			outpostTurrets = globalState.outpostTurrets;
		}

		void InitializeGlobalState() {
			AllActiveTurretsGlobalState existingGlobalState = FindObjectOfType<AllActiveTurretsGlobalState>();
			if (existingGlobalState != null) {
				globalState = existingGlobalState;
				return;
			}

			globalState = new GameObject("AllActiveTurretsGlobalState").AddComponent<AllActiveTurretsGlobalState>();
			globalState.updateIntervalInSeconds = updateIntervalInSeconds;
		}

		public List<OutpostTurret> GetAllTurretsOfAllegiance(Allegiance allegiance) {
			List<OutpostTurret> targetTurretList = new List<OutpostTurret>();
			foreach (var turret in outpostTurrets) {
				if (turret.allegiance == allegiance) {
					targetTurretList.Add(turret);
				}
			}
			return targetTurretList;
		}

		public OutpostTurret GetClosestTurretFromList(List<OutpostTurret> turrets = null, Vector3? relativePosition = null) {
			var selectedRelativePosition = relativePosition ?? transform.position;

			return (turrets ?? outpostTurrets)
				.OrderBy(t=>(t.transform.position - selectedRelativePosition).sqrMagnitude)
				.First();
		}

		public OutpostTurret GetClosestTurretOfAllegiance(Allegiance allegiance) {
			var turrets = GetAllTurretsOfAllegiance(allegiance);
			if (turrets.Count > 0) {
				return GetClosestTurretFromList(turrets);
			}

			return null;
		}

		class AllActiveTurretsGlobalState : MonoBehaviour
		{
			private static AllActiveTurretsGlobalState _instance;
			public static AllActiveTurretsGlobalState Instance { get { return _instance; } }

			private void Awake()
			{
				if (_instance != null && _instance != this)
				{
					Destroy(this.gameObject);
				} else {
					_instance = this;
				}

				StartCoroutine(UpdateTurretInformation());
			}

			[ReadOnly] public List<OutpostTurret> outpostTurrets = new List<OutpostTurret>();
			[ReadOnly] public int updateIntervalInSeconds;

			IEnumerator UpdateTurretInformation() {
				while (true) {
					outpostTurrets.Clear();
					outpostTurrets.AddRange(FindObjectsOfType<OutpostTurret>());

					yield return new WaitForSeconds(updateIntervalInSeconds);
				}
			}
		}
	}
}
