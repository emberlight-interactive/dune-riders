using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class TurretPistolGunFiringController : MonoBehaviour
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
				UpdateGunStateToFiring();
				StartFiring();
			} else if (!ShootButtonPressed() && IsTheGunCurrentlyFiring()) {
				UpdateGunStateToReloading();
				StopFiring();
			}
		}

		public void StartFiring() {
			var spawnedProjectile = SimplePool.Spawn(projectile, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
			SimplePool.Despawn(spawnedProjectile, 4f);
		}

		public void StopFiring() {
			StartCoroutine(Reload(timeToReload));
		}

		bool ShootButtonPressed() {
			return triggerPulled;
		}

		public void TriggerPulled() {
			triggerPulled = true;
		}

		public void TriggerReleased() {
			triggerPulled = false;
		}

		bool CanTheGunFire() {
			return gunState.availableActions.canFire;
		}

		bool IsTheGunCurrentlyFiring() {
			return gunState.state == GunState.State.Firing;
		}

		void UpdateGunStateToFiring() {
			gunState.state = GunState.State.Firing;
		}

		void UpdateGunStateToReloading() {
			gunState.state = GunState.State.Reloading;
		}

		IEnumerator Reload(float timeToReload) {
			yield return new WaitForSeconds(timeToReload);
			gunState.state = GunState.State.Ready;
		}
	}
}
