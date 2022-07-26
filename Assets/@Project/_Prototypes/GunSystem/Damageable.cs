using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class Damageable : MonoBehaviour
	{
		[SerializeField] int healthPoints;
		public int HealthPoints {get => healthPoints; }

		public void Damage(int healthPoints)
		{
			this.healthPoints -= healthPoints;
		}
	}
}
