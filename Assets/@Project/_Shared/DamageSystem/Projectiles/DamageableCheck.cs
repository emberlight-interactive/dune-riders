using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Shared.DamageSystem {
	public abstract class DamageableCheck : MonoBehaviour
	{
		public abstract bool CanDamage(Damageable damageable);
	}
}
