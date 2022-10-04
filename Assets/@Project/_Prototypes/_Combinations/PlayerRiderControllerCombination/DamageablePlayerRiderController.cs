using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DuneRiders.GunSystem;
using DuneRiders.DrivingSystem;
using DuneRiders.Shared.DamageSystem;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class DamageablePlayerRiderController : Damageable, IPersistent
	{
		[Serializable]
		class DamageablePlayerRiderControllerSerializable {
			public int health;
		}

		[SerializeField] DashboardHealthMonitor dashboardHealthMonitor;
		[SerializeField] int health = 100;
		[SerializeField] UnityEvent onZeroHealthPoints = new UnityEvent();
		public bool DisablePersistence { get => false; }
		string persistenceKey = "PlayerHealth";

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

		public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save<DamageablePlayerRiderControllerSerializable>(persistenceKey, new DamageablePlayerRiderControllerSerializable {
				health = this.health,
			});
		}

		// todo: does loading the halt command twice move it to new halt positions?
		public void Load(IPersistenceUtil persistUtil) {
			var loadedDamageablePlayerRiderController = persistUtil.Load<DamageablePlayerRiderControllerSerializable>(persistenceKey);
			health = loadedDamageablePlayerRiderController.health;
		}
	}
}
