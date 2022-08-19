using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GunSystem;
using DuneRiders.DrivingSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class DamageablePlayerRiderController : Damageable
	{
		[SerializeField] DashboardHealthMonitor dashboardHealthMonitor;

		[SerializeField] int health = 100;

		public override void Damage(int healthPoints)
		{
			health -= healthPoints;
			UpdateMonitor();
		}

		void UpdateMonitor() {
			dashboardHealthMonitor.Health = health;
		}

		public void Heal(int healthPoints) {
			health += healthPoints;
			UpdateMonitor();
		}

		public int Health() {
			return health;
		}
	}
}
