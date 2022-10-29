using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI {
	public class ProjectileProvider : MonoBehaviour
	{
		[SerializeField] protected GameObject friendlyRiderProjectile;
		[SerializeField] protected GameObject enemyRiderProjectile;
		Rider rider;

		void Awake() {
			rider = GetComponentInParent<Rider>();
		}

		public virtual GameObject Projectile { get => rider.allegiance == Allegiance.Player ? friendlyRiderProjectile : enemyRiderProjectile; }
	}
}
