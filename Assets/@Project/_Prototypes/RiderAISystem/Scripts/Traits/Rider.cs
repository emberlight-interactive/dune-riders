using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.Traits {
	[DisallowMultipleComponent]
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

		public Allegiance allegiance = Allegiance.Mercenary;
		public Allegiance enemyAllegiance = Allegiance.Bandits;
		public ChasisType chasisType = ChasisType.Heavy;
		public GunType gunType = GunType.Cannon;
	}
}
