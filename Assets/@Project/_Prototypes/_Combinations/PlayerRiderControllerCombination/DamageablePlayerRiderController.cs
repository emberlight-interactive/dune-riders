using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DuneRiders.GunSystem;
using DuneRiders.DrivingSystem;
using DuneRiders.Shared.DamageSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class DamageablePlayerRiderController : Damageable
	{
		[SerializeField] DashboardHealthMonitor dashboardHealthMonitor;
		[SerializeField] int health = 100;
		[SerializeField] UnityEvent onZeroHealthPoints = new UnityEvent();

		public override void Damage(int healthPoints)
		{
			health -= healthPoints;
			UpdateMonitor();
			HandleZeroHealthPoints();
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

		void HandleZeroHealthPoints() {
			if (health <= 0) {
				onZeroHealthPoints.Invoke();
			}
		}
	}
}
