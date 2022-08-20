using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DuneRiders.OutpostAI.State;

namespace DuneRiders.OutpostAICombination {
	public class HealthStateToHealthBar : MonoBehaviour
	{
		[SerializeField] HealthState healthState;
		[SerializeField] Slider healthBar;
		float initialHealthState;

		void Start() {
			initialHealthState = healthState.health;
		}

		void Update() {
			healthBar.value = healthState.health / initialHealthState;
		}
	}
}
