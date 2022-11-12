using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;
using DuneRiders.OutpostAI.Traits;
using DuneRiders.Shared.DamageSystem;

namespace DuneRiders.RiderAI {
	public class EnemyOutpostTurretProjectileDamageableCheck : DamageableCheck
	{
		public override bool CanDamage(Damageable damageable) {
			var rider = damageable.gameObject.GetComponentInParent<Rider>();
			if (rider != null && rider.allegiance == Allegiance.Bandits) return false;

			var outpost = damageable.gameObject.GetComponentInParent<Outpost>();
			if (outpost != null && outpost.allegiance == Allegiance.Bandits) return false;

			var turret = damageable.gameObject.GetComponentInParent<OutpostTurret>();
			if (turret != null && turret.allegiance == Allegiance.Bandits) return false;

			return true;
		}
	}
}
