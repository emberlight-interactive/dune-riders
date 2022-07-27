using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class MissileLauncherFiringController : MonoBehaviour
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
			return shootInput.action.ReadValue<float>() > 0;
		}

		bool CanTheGunFire() {
			return gunState.availableActions.canFire;
		}

		IEnumerator Reload(float timeToReload) {
			gunState.state = GunState.State.Reloading;
			yield return new WaitForSeconds(timeToReload);
			gunState.state = GunState.State.Ready;
		}
	}
}
