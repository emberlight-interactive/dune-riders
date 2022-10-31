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
			var prevHealth = health;
			health -= healthPoints;
			UpdateMonitor();
			if (prevHealth > 0 && health <= 0) HandleZeroHealthPoints();
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

		public void Load(IPersistenceUtil persistUtil) {
			var loadedDamageablePlayerRiderController = persistUtil.Load<DamageablePlayerRiderControllerSerializable>(persistenceKey);
			health = loadedDamageablePlayerRiderController.health;
			UpdateMonitor();
		}
	}
}
