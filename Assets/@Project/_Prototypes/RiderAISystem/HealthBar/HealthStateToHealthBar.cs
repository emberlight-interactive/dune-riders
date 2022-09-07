using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI.HealthBar {
	public class HealthStateToHealthBar : MonoBehaviour
	{
		[SerializeField] HealthState healthState;
		[SerializeField] Slider healthBar;

		void Update() {
			healthBar.value = (float) healthState.health / healthState.MaxHealth;
		}
	}
}
