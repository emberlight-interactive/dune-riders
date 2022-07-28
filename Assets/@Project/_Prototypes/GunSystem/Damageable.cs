using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GunSystem {
	public abstract class Damageable : MonoBehaviour
	{
		public abstract void Damage(int healthPoints);
	}
}
