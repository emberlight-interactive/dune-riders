using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DuneRiders.Shared.DamageSystem {
	public class EventOnDamage : Damageable
	{
		public UnityEvent damageEvent = new UnityEvent();

		public override void Damage(float healthPoints) {
			damageEvent.Invoke();
		}
	}
}
