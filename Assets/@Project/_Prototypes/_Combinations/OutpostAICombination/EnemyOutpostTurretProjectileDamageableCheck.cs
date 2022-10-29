using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;
using DuneRiders.Shared.DamageSystem;

namespace DuneRiders.RiderAI {
	public class EnemyOutpostTurretProjectileDamageableCheck : DamageableCheck
	{
		public override bool CanDamage(Damageable damageable) {
			var rider = damageable.gameObject.GetComponentInParent<Rider>();
			if (rider == null) return true;

			if (rider.allegiance == Allegiance.Bandits) return false;
			return true;
		}
	}
}
