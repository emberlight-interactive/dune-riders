using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.OutpostAI {
	public class StructureHealth : MonoBehaviour
	{
		[Serializable]
		class StructureHealthState {
			public float health;
		}

		[SerializeField, ReadOnly] StructureHealthState state;
		[SerializeField] float maxHealth = 50;
		public float MaxHealth { get => maxHealth; }

		public UnityEvent deathEvent = new UnityEvent();

		ProceduralTools proceduralTools;
		public float health { get => state.health; set => state.health = value; }

		void Awake() {
			proceduralTools = new ProceduralTools(transform, true);
			GlobalState.InitState<StructureHealthGlobalState, string, StructureHealthState>(
				proceduralTools.BuildTransformHash(),
				new StructureHealthState() { health = maxHealth },
				out state,
				new Type[] { typeof(LoadLocalComponentsOnAwake) }
			);

			ManageDeathLol(true);
		}

		void FixedUpdate() {
			ManageDeathLol();
		}

		void ManageDeathLol(bool ignoreEvents = false) {
			if (health <= 0) {
				gameObject.SetActive(false);
				if (!ignoreEvents) deathEvent.Invoke();
			}
		}

		class StructureHealthGlobalState : GlobalStateGameObject<string, StructureHealthState> {}
	}
}
