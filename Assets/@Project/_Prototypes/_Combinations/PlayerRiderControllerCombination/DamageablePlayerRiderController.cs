using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GunSystem;
using DuneRiders.DrivingSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class DamageablePlayerRiderController : Damageable
	{
		[SerializeField] DashboardHealthMonitor dashboardHealthMonitor;

		int health = 100;

		public override void Damage(int healthPoints)
		{
			health -= healthPoints;
			UpdateMonitor();
		}

		void UpdateMonitor() {
			dashboardHealthMonitor.Health = health;
		}
	}
}