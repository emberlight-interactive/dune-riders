using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	public class HealthState : MonoBehaviour
	{
		[SerializeField] int maxHealth = 100;
		public int MaxHealth { get => maxHealth; }

		[ReadOnly] public int health;

		void Awake() {
			if (health == default(int)) health = maxHealth;
		}
	}
}
