using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI {
	public class ProjectileProvider : MonoBehaviour
	{
		[SerializeField] GameObject friendlyRiderProjectile;
		[SerializeField] GameObject enemyRiderProjectile;
		Rider rider;

		void Awake() {
			rider = GetComponentInParent<Rider>();
		}

		public GameObject Projectile { get => rider.allegiance == Allegiance.Player ? friendlyRiderProjectile : enemyRiderProjectile; }
	}
}
