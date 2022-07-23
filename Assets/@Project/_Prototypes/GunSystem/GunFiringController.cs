using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class GunFiringController : MonoBehaviour
	{
		[SerializeField] private InputActionProperty shootInput;
		[SerializeField] float timeToReload = 2f;
		[SerializeField] Transform projectileSpawnLocation;
		[SerializeField] GameObject projectile;
		GunState gunState;


		void Start() {
			shootInput.action.Enable();
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
			(Camera.main.gameObject.GetComponent<CoroutineParasite>() ?? Camera.main.gameObject.AddComponent<CoroutineParasite>()).StartCoroutine(DespawnBullet(spawnedProjectile, 4f));
		}

		public void StopFiring() {
			StartCoroutine(Reload(timeToReload));
		}

		bool ShootButtonPressed() {
			return shootInput.action.ReadValue<float>() > 0;
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

		IEnumerator DespawnBullet(GameObject bullet, float delayInSeconds = 0f) {
			yield return new WaitForSeconds(delayInSeconds);
			SimplePool.Despawn(bullet);
		}
	}
}
