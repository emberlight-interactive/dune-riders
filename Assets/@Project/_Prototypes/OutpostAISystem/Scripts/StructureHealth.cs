using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace DuneRiders.OutpostAI {
	public class StructureHealth : MonoBehaviour
	{
		[Serializable]
		class StructureHealthState {
			public int health;
		}

		[SerializeField, ReadOnly] StructureHealthState state;
		[SerializeField] int maxHealth = 50;
		public int MaxHealth { get => maxHealth; }

		public UnityEvent deathEvent = new UnityEvent();

		ProceduralTools proceduralTools;
		public int health { get => state.health; set => state.health = value; }

		void Awake() {
			proceduralTools = new ProceduralTools(transform, true);
			GlobalState.InitState<StructureHealthGlobalState, string, StructureHealthState>(
				proceduralTools.BuildTransformHash(),
				new StructureHealthState() { health = maxHealth },
				out state,
				new Type[] { typeof(StructureHealthGlobalStatePersistence) }
			);

			ManageDeathLol();
		}

		void FixedUpdate() {
			ManageDeathLol();
		}

		void ManageDeathLol() {
			if (health <= 0) {
				gameObject.SetActive(false);
				deathEvent.Invoke();
			}
		}

		class StructureHealthGlobalState : GlobalStateGameObject<string, StructureHealthState> {}
		class StructureHealthGlobalStatePersistence : GlobalStatePersistence<StructureHealthState> {}
	}
}
