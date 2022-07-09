using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI.Traits {
	public class Rider : MonoBehaviour
	{
		public enum ChasisType {
			Light,
			Normal,
			Heavy
		}

		public enum GunType {
			MachineGun,
			TripleMissileLauncher,
			Cannon
		}

		public enum Allegiance {
			Player,
			Bandits,
			Mercenary,
		}

		public Allegiance allegiance = Allegiance.Mercenary;
		public Allegiance enemyAllegiance = Allegiance.Bandits;
		public ChasisType chasisType = ChasisType.Heavy;
		public GunType gunType = GunType.Cannon;
	}
}
