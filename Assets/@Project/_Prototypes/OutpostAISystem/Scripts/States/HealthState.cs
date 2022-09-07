using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.OutpostAI.State {
	[DisallowMultipleComponent]
	public class HealthState : MonoBehaviour
	{
		[Serializable]
		class TurretHealthState {
			public int health;
		}

		[SerializeField, ReadOnly] TurretHealthState state;
		ProceduralTools proceduralTools;
		[SerializeField] int maxHealth = 100;
		public int MaxHealth { get => maxHealth; }

		public int health { get => state.health; set => state.health = value; }

		void Awake() {
			proceduralTools = new ProceduralTools(transform);
			GlobalState.InitState<TurretHealthGlobalState, string, TurretHealthState>(
				proceduralTools.BuildTransformHash(),
				new TurretHealthState() { health = maxHealth },
				out state
			);
		}

		class TurretHealthGlobalState : GlobalStateGameObject<string, TurretHealthState> {}
	}
}
