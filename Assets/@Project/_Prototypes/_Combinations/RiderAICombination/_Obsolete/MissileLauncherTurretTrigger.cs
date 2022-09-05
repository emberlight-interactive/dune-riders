using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;

namespace DuneRiders.RiderAICombination {
	[System.Obsolete()]
	public class MissileLauncherTurretTrigger : TurretTrigger
	{
		[SerializeField] GameObject projectile;
		[SerializeField] Transform projectileSpawnLocation;
		[SerializeField] float timeToReload = 2f;
		bool reloading = false;

		public override void PullTrigger() {
			if (!reloading) {
				StartCoroutine(FireMissiles());
				reloading = true;
			}
		}

		public override void ReleaseTrigger() {}

		IEnumerator FireMissiles() {
			foreach (var i in System.Linq.Enumerable.Range(0, 3))
			{
				var spawnedProjectile = SimplePool.Spawn(projectile, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
				SimplePool.Despawn(spawnedProjectile, 4f);
				yield return new WaitForSeconds(.2f);
			}

			StartCoroutine(Reload());
		}

		IEnumerator Reload() {
			yield return new WaitForSeconds(timeToReload);
			reloading = false;
		}
	}
}
