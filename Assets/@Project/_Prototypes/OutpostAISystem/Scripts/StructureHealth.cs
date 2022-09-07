using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.OutpostAI {
	public class StructureHealth : MonoBehaviour
	{
		[Serializable]
		class StructureHealthState {
			public int health;
		}

		[SerializeField, ReadOnly] StructureHealthState state;
		ProceduralTools proceduralTools;
		public int health { get => state.health; set => state.health = value; }

		void Awake() {
			proceduralTools = new ProceduralTools(transform);
			GlobalState.InitState<StructureHealthGlobalState, string, StructureHealthState>(
				proceduralTools.BuildTransformHash(),
				new StructureHealthState() { health = 50 },
				out state
			);
		}

		void FixedUpdate() {
			if (health <= 0) gameObject.SetActive(false);
		}

		class StructureHealthGlobalState : GlobalStateGameObject<string, StructureHealthState> {}
	}
}
