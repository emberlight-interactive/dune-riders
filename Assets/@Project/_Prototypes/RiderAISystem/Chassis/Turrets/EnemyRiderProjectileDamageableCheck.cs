using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;
using DuneRiders.Shared.DamageSystem;

namespace DuneRiders.RiderAI { // todo: Eventually when riders need to defend against other types of friendly fire this will need to be shifted to combinations and modified projectiles with these checks will need ot be "hacked" in during Start()
	public class EnemyRiderProjectileDamageableCheck : DamageableCheck
	{
		public override bool CanDamage(Damageable damageable) {
			var rider = damageable.gameObject.GetComponentInParent<Rider>();
			if (rider == null) return true;

			if (rider.allegiance == Allegiance.Bandits) return false;
			return true;
		}
	}
}
