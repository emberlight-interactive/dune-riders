using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GunSystem;
using DuneRiders.OutpostAI.State;

namespace DuneRiders.OutpostAICombination {
	public class DamageableOutpostTurret : Damageable
	{
		[SerializeField] HealthState healthState;

		public override void Damage(int healthPoints)
		{
			healthState.health -= healthPoints;
		}
	}
}
