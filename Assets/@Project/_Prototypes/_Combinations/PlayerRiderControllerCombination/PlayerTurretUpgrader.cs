using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GunSystem;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class PlayerTurretUpgrader : MonoBehaviour, IPersistent
	{
		[Serializable]
		class PlayerTurretUpgraderSerializable {
			public UpgradeType[] appliedUpgrades;
		}

		[Serializable]
		public enum UpgradeType {
			Rockets,
			MachineGun,
		}

		[Serializable]
		class Upgrades {
			public UpgradeType upgradeType;
			public TurretPistolGunSwapController.Gun gunInfo;
		}

		[SerializeField] TurretPistolGunSwapController gunSwapper;
		[SerializeField] List<Upgrades> upgrades = new List<Upgrades>();
		List<UpgradeType> appliedUpgrades = new List<UpgradeType>();
		public bool DisablePersistence { get => false; }
		string persistenceKey = "PlayerTurretUpgrader";

		/// <summary>
		///	Idempotent
		/// </summary>
		public void ApplyUpgrade(UpgradeType upgradeType) {
			if (HasUpgradeBeenApplied(upgradeType)) return;

			var gunInfo = upgrades.Where((upgrades) => upgrades.upgradeType == upgradeType).Select((upgrades) => upgrades.gunInfo).First();
			gunSwapper.gunsToSwapBetween.Add(gunInfo);
			appliedUpgrades.Add(upgradeType);
		}

		public bool HasUpgradeBeenApplied(UpgradeType upgradeType) {
			return appliedUpgrades.Exists((thisUpgradeType) => thisUpgradeType == upgradeType);
		}

		public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save(persistenceKey, new PlayerTurretUpgraderSerializable {
				appliedUpgrades = appliedUpgrades.ToArray(),
			});
		}

        public void Load(IPersistenceUtil persistUtil) {
			var loadedClass = persistUtil.Load<PlayerTurretUpgraderSerializable>(persistenceKey);
			foreach (var upgradeType in loadedClass.appliedUpgrades) {
				ApplyUpgrade(upgradeType);
			}
		}
	}
}
