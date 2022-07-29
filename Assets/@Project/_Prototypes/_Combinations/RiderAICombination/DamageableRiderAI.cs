using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GunSystem;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAICombination {
	public class DamageableRiderAI : Damageable
	{
		[SerializeField] HealthState healthState;

		public override void Damage(int healthPoints)
		{
			healthState.health -= healthPoints;
		}
	}
}
