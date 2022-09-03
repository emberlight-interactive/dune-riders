using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.Shared.DamageSystem;

namespace DuneRiders.GunSystem {
	public class DamageableTest : Damageable
	{
		[SerializeField] int healthPoints;
		public int HealthPoints {get => healthPoints; }

		public override void Damage(int healthPoints)
		{
			this.healthPoints -= healthPoints;
		}
	}
}
