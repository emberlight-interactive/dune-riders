using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class TurretPistolMissileLauncherFiringController : MonoBehaviour
	{
		[SerializeField] float timeToReload = 2f;
		[SerializeField] Transform projectileSpawnLocation;
		[SerializeField] GameObject projectile;
		GunState gunState;
		bool triggerPulled = false;

		void Start() {
			gunState = GetComponent<GunState>();
		}

		void Update() {
			if (ShootButtonPressed() && CanTheGunFire()) {
				gunState.state = GunState.State.Firing;
				StartCoroutine(FireMissiles());
			}
		}

		IEnumerator FireMissiles() {
			foreach (var i in System.Linq.Enumerable.Range(0, 3))
			{
				var spawnedProjectile = SimplePool.Spawn(projectile, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
				SimplePool.Despawn(spawnedProjectile, 4f);
				yield return new WaitForSeconds(.2f);
			}

			StartCoroutine(Reload(timeToReload));
		}

		bool ShootButtonPressed() {
			return triggerPulled;
		}

		bool CanTheGunFire() {
			return gunState.availableActions.canFire;
		}

		public void TriggerPulled() {
			triggerPulled = true;
		}

		public void TriggerReleased() {
			triggerPulled = false;
		}

		IEnumerator Reload(float timeToReload) {
			gunState.state = GunState.State.Reloading;
			yield return new WaitForSeconds(timeToReload);
			gunState.state = GunState.State.Ready;
		}
	}
}
