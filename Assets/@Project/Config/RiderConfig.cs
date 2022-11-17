using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.Config {
	[CreateAssetMenu(fileName = "RiderConfig", menuName = "ScriptableObjects/RiderConfig", order = 1)]
	public class RiderConfig : ScriptableObject
	{
		[Serializable]
		public class MercenaryConfig {
			public float preciousMetalCost;
			public float availabilityValue;
		}

		[Header("Health Settings")]
		public float friendlyHeavyChassisHealth;
		public float enemyHeavyChassisHealth;

		public float friendlyNormalChassisHealth;
		public float enemyNormalChassisHealth;

		public float friendlyLightChassisHealth;
		public float enemyLightChassisHealth;

		[Header("Fuel Usage Settings")]
		public float heavyChassisFuelPerHour;
		public float normalChassisFuelPerHour;
		public float lightChassisFuelPerHour;

		[Header("Chassis Sprite Settings")]
		public Sprite heavyChassisSprite;
		public Sprite normalChassisSprite;
		public Sprite lightChassisSprite;

		[Header("Turret Sprite Settings")]
		public Sprite cannonSprite;
		public Sprite missilesSprite;
		public Sprite turretSprite;

		[Header("Mercenary Settings")]
		public MercenaryConfig mercenaryLightChassisTurret;
		public MercenaryConfig mercenaryLightChassisMissiles;
		public MercenaryConfig mercenaryNormalChassisTurret;
		public MercenaryConfig mercenaryNormalChassisMissiles;
		public MercenaryConfig mercenaryNormalChassisCannon;
		public MercenaryConfig mercenaryHeavyChassisMissiles;
		public MercenaryConfig mercenaryHeavyChassisCannon;

		public float FriendlyRiderAIChassisToHealth(Rider.ChasisType chasisType) {
			switch (chasisType) {
				case Rider.ChasisType.Light:
					return friendlyLightChassisHealth;
				case Rider.ChasisType.Normal:
					return friendlyNormalChassisHealth;
				case Rider.ChasisType.Heavy:
					return friendlyHeavyChassisHealth;
				default:
					return friendlyLightChassisHealth;
			}
		}

		public float EnemyRiderAIChassisToHealth(Rider.ChasisType chasisType) {
			switch (chasisType) {
				case Rider.ChasisType.Light:
					return enemyLightChassisHealth;
				case Rider.ChasisType.Normal:
					return enemyNormalChassisHealth;
				case Rider.ChasisType.Heavy:
					return enemyHeavyChassisHealth;
				default:
					return enemyLightChassisHealth;
			}
		}

		public Sprite RiderAIChassisToSprite(Rider.ChasisType chasisType) {
			switch (chasisType) {
				case Rider.ChasisType.Light:
					return lightChassisSprite;
				case Rider.ChasisType.Normal:
					return normalChassisSprite;
				case Rider.ChasisType.Heavy:
					return heavyChassisSprite;
				default:
					return lightChassisSprite;
			}
		}

		public Sprite RiderAIGunToSprite(Rider.GunType gunType) {
			switch (gunType) {
				case Rider.GunType.MachineGun:
					return turretSprite;
				case Rider.GunType.TripleMissileLauncher:
					return missilesSprite;
				case Rider.GunType.Cannon:
					return cannonSprite;
				default:
					return turretSprite;
			}
		}

		public string RiderAIGunToName(Rider.GunType gunType) {
			switch (gunType) {
				case Rider.GunType.MachineGun:
					return "Turret";
				case Rider.GunType.TripleMissileLauncher:
					return "Missiles";
				case Rider.GunType.Cannon:
					return "Cannon";
				default:
					return "Turret";
			}
		}
	}
}
