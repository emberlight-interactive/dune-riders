using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.Config;
using DuneRiders.AI;
using DuneRiders.Shared.PersistenceSystem;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI.State {
	[RequireComponent(typeof(Rider))]
	[RequireComponent(typeof(UniqueIdentifier))]
	[DisallowMultipleComponent]
	public class HealthState : MonoBehaviour, IPersistent
	{
		[Serializable]
		class HealthStateSerializable {
			public float health;
		}

		[SerializeField] RiderConfig riderConfig;
		public float MaxHealth { get => maxHealth; }
		public bool DisablePersistence { get => false; }

		[ReadOnly] float maxHealth = 100;
		[ReadOnly] public float health;

		Rider rider;

		void Awake() {
			rider = GetComponent<Rider>();

			if (rider.allegiance == Allegiance.Player) maxHealth = riderConfig.FriendlyRiderAIChassisToHealth(rider.chasisType);
			else if (rider.allegiance == Allegiance.Bandits) maxHealth = riderConfig.EnemyRiderAIChassisToHealth(rider.chasisType);

			health = maxHealth;
		}

		public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save(PersistenceKey(), new HealthStateSerializable {
				health = this.health,
			});
		}

        public void Load(IPersistenceUtil persistUtil) {
			var loadedHealthState = persistUtil.Load<HealthStateSerializable>(PersistenceKey());
			health = loadedHealthState.health;
		}

		string PersistenceKey() {
			return $"RiderHealth-{GetComponent<UniqueIdentifier>().uniqueIdentifier}";
		}
	}
}
