using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Shared.DamageSystem {
	public abstract class Damageable : MonoBehaviour
	{
		public abstract void Damage(float healthPoints);
	}
}
