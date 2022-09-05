using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;

namespace DuneRiders.RiderAICombination {
	[System.Obsolete()]
	public class CannonTurretTrigger : TurretTrigger
	{
		[SerializeField] GameObject projectile;
		[SerializeField] Transform projectileSpawnLocation;
		[SerializeField] float timeToReload = 2f;
		bool reloading = false;

		public override void PullTrigger() {
			if (!reloading) {
				Fire();
				reloading = true;
				StartCoroutine(Reload());
			}
		}

		public override void ReleaseTrigger() {}

		void Fire() {
			var spawnedProjectile = SimplePool.Spawn(projectile, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
			SimplePool.Despawn(spawnedProjectile, 4f);
		}

		IEnumerator Reload() {
			yield return new WaitForSeconds(timeToReload);
			reloading = false;
		}
	}
}
