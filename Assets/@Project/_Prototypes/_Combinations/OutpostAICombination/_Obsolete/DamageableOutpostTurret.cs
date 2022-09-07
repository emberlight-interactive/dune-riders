using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GunSystem;
using DuneRiders.OutpostAI.State;
using DuneRiders.Shared.DamageSystem;

namespace DuneRiders.OutpostAICombination {
	[System.Obsolete]
	public class DamageableOutpostTurret : Damageable
	{
		[SerializeField] HealthState healthState;

		public override void Damage(int healthPoints)
		{
			healthState.health -= healthPoints;
		}
	}
}
