using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI {
	// todo: I forsee this becoming a more generic "RiderInstanceBuilder"
	public class EnemyRiderInstanceBuilder : MonoBehaviour
	{
		public struct RiderConfig {
			public Rider.ChasisType chasisType;
			public Rider.GunType gunType;
		}

		Allegiance allegiance = Allegiance.Bandits;
		Allegiance enemyAllegiance = Allegiance.Player;

		[SerializeField] Rider enemyRiderPrefab;
		List<RiderConfig> riderVariants = new List<RiderConfig>() {
			new RiderConfig { chasisType = Rider.ChasisType.Light, gunType = Rider.GunType.MachineGun },
			new RiderConfig { chasisType = Rider.ChasisType.Light, gunType = Rider.GunType.TripleMissileLauncher },
			new RiderConfig { chasisType = Rider.ChasisType.Normal, gunType = Rider.GunType.MachineGun },
			new RiderConfig { chasisType = Rider.ChasisType.Normal, gunType = Rider.GunType.TripleMissileLauncher },
			new RiderConfig { chasisType = Rider.ChasisType.Normal, gunType = Rider.GunType.Cannon },
			new RiderConfig { chasisType = Rider.ChasisType.Heavy, gunType = Rider.GunType.TripleMissileLauncher },
			new RiderConfig { chasisType = Rider.ChasisType.Heavy, gunType = Rider.GunType.Cannon },
		};

		public Rider GetRandomRider() {
			var randomRiderConfig = riderVariants[Random.Range(0, riderVariants.Count)];

			enemyRiderPrefab.allegiance = allegiance;
			enemyRiderPrefab.enemyAllegiance = enemyAllegiance;
			enemyRiderPrefab.chasisType = randomRiderConfig.chasisType;
			enemyRiderPrefab.gunType = randomRiderConfig.gunType;

			return enemyRiderPrefab;
		}

		public GameObject BuildRiderInstance(RiderConfig rider) {
			enemyRiderPrefab.allegiance = allegiance;
			enemyRiderPrefab.enemyAllegiance = enemyAllegiance;
			enemyRiderPrefab.chasisType = rider.chasisType;
			enemyRiderPrefab.gunType = rider.gunType;

			return enemyRiderPrefab.gameObject;
		}
	}
}
