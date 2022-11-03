using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.RiderAI.State {
	[RequireComponent(typeof(UniqueIdentifier))]
	[DisallowMultipleComponent]
	public class HealthState : MonoBehaviour, IPersistent
	{
		[Serializable]
		class HealthStateSerializable {
			public float health;
		}

		[SerializeField] float maxHealth = 100;
		public float MaxHealth { get => maxHealth; }
		public bool DisablePersistence { get => false; }

		[ReadOnly] public float health;

		void Awake() {
			if (health == default(float)) health = maxHealth;
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
