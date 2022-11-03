using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GunSystem;
using DuneRiders.RiderAI.State;
using DuneRiders.Shared.DamageSystem;

namespace DuneRiders.RiderAICombination {
	[System.Obsolete()]
	public class DamageableRiderAI : Damageable
	{
		[SerializeField] HealthState healthState;

		public override void Damage(float healthPoints)
		{
			healthState.health -= healthPoints;
		}
	}
}
