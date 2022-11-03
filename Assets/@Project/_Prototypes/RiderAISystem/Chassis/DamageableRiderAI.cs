using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.State;
using DuneRiders.Shared.DamageSystem;

namespace DuneRiders.RiderAI {
	public class DamageableRiderAI : Damageable
	{
		[SerializeField] HealthState healthState;

		public HealthState HealthState { set {
				healthState = value;
			}
		}

		public override void Damage(float healthPoints)
		{
			healthState.health -= healthPoints;
		}
	}
}
