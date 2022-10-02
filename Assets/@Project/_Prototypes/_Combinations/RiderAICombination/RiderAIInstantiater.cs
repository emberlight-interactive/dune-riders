using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;
using DuneRiders.Shared.PersistenceSystem;
using DuneRiders.PersistenceSystemCombination;

namespace DuneRiders.RiderAICombination {
	public class RiderAIInstantiater : MonoBehaviour
	{
		[SerializeField] Rider mercenaryPrefab;
		[SerializeField] Rider friendlyPrefab;
		[SerializeField] Rider enemyPrefab;

		Rider rider;
		PrefabInstanceTag prefabInstanceTag;

		void Awake() {
			rider = GetComponent<Rider>();
			prefabInstanceTag = GetComponent<PrefabInstanceTag>();
		}

		void Start() {
			var riderPrefab = GetRiderPrefab();
			riderPrefab.CopyFields(rider);

			DestroyImmediate(rider);

			var gm = Instantiate(riderPrefab.gameObject);
			gm.GetComponent<PrefabInstanceTag>().prefabInstanceKey = prefabInstanceTag.prefabInstanceKey;
			gm.AddComponent<LoadLocalComponentsOnAwake>();

			Destroy(gameObject);
		}

		Rider GetRiderPrefab() {
			switch (rider.allegiance) {
				case Allegiance.Mercenary:
					return mercenaryPrefab;
				case Allegiance.Player:
					return friendlyPrefab;
				case Allegiance.Bandits:
					return enemyPrefab;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
