using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;

namespace DuneRiders.RiderAI {
	[RequireComponent(typeof(ProjectileProvider))]
	public class CannonTurretTrigger : TurretTrigger
	{
		ProjectileProvider projectileProvider;
		[SerializeField] Transform projectileSpawnLocation;
		[SerializeField] float timeToReload = 2f;
		bool reloading = false;

		void Awake() {
			projectileProvider = GetComponent<ProjectileProvider>();
		}

		public override void PullTrigger() {
			if (!reloading) {
				Fire();
				reloading = true;
				StartCoroutine(Reload());
			}
		}

		public override void ReleaseTrigger() {}

		void Fire() {
			var spawnedProjectile = SimplePool.Spawn(projectileProvider.Projectile, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
			SimplePool.Despawn(spawnedProjectile, 4f);
		}

		IEnumerator Reload() {
			yield return new WaitForSeconds(timeToReload);
			reloading = false;
		}
	}
}
