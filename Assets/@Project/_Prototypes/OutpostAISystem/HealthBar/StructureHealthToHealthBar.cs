using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuneRiders.OutpostAI {
	public class StructureHealthToHealthBar : MonoBehaviour
	{
		[SerializeField] StructureHealth structureHealth;
		[SerializeField] Slider healthBar;
		float initialHealthState;

		void Start() {
			initialHealthState = structureHealth.health;
		}

		void Update() {
			healthBar.value = structureHealth.health / initialHealthState;
		}
	}
}
