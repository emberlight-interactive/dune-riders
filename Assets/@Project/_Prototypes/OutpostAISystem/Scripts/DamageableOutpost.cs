using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GunSystem;
using DuneRiders.OutpostAI;
using DuneRiders.Shared.DamageSystem;

namespace DuneRiders.OutpostAI {
	public class DamageableOutpost : Damageable
	{
		[SerializeField] StructureHealth healthState;

		public override void Damage(int healthPoints)
		{
			healthState.health -= healthPoints;
		}
	}
}
