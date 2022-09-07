using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.OutpostAI {
	public class StructureHealth : MonoBehaviour
	{
		[Serializable]
		class StructureHealthState {
			public int health;
		}

		StructureHealthState state;
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
			// todo: Do the turrets still register after destruction ??
			// Might need to create a "structure" state that monitors if it's host structure is deactivated
			// and add that to the behaviour tree
			if (health <= 0) gameObject.SetActive(false);
		}

		class StructureHealthGlobalState : GlobalStateGameObject<string, StructureHealthState> {}
	}
}
