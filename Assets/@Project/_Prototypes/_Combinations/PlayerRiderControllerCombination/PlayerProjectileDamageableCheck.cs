using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;
using DuneRiders.Shared.DamageSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class PlayerProjectileDamageableCheck : DamageableCheck
	{
		public override bool CanDamage(Damageable damageable) {
			var rider = damageable.gameObject.GetComponentInParent<Rider>();
			if (rider == null) return true;

			if (rider.allegiance == Allegiance.Player) return false;
			return true;
		}
	}
}
