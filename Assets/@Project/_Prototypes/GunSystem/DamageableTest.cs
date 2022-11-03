using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.Shared.DamageSystem;

namespace DuneRiders.GunSystem {
	public class DamageableTest : Damageable
	{
		[SerializeField] float healthPoints;
		public float HealthPoints {get => healthPoints; }

		public override void Damage(float healthPoints)
		{
			this.healthPoints -= healthPoints;
		}
	}
}
