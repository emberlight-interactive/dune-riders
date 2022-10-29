using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI;
using DuneRiders.OutpostAI.Traits;

namespace DuneRiders.OutpostAICombination {
	public class TurretProjectileProvider : ProjectileProvider
	{
		Outpost outpost;

		void Awake() {
			outpost = GetComponentInParent<Outpost>();
		}

		public override GameObject Projectile { get => outpost.allegiance == Allegiance.Player ? friendlyRiderProjectile : enemyRiderProjectile; }
	}
}
