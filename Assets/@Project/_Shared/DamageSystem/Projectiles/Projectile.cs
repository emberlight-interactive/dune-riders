using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Shared.DamageSystem {
	public class Projectile : MonoBehaviour
	{
		public DamageableCheck damageableCheck;

		protected bool CanDamage(Damageable damageable) {
			if (damageableCheck == null) return true;
			return damageableCheck.CanDamage(damageable);
		}
	}
}
