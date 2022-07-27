using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class MachineGunFiringController : MonoBehaviour
	{
		[SerializeField] private InputActionProperty shootInput;
		[SerializeField] float coolDownMultiplier = 1f;
		[SerializeField] int maxFirableMachineGunBullets = 20;
		[SerializeField] Transform projectileSpawnLocation;
		[SerializeField] GameObject projectile;
		Coroutine firingRoutine;
		GunState gunState;
		int bulletsFiredCounter = 0;

		void Start() {
			shootInput.action.Enable();
			gunState = GetComponent<GunState>();
		}

		void Update() {
			if (ShootButtonPressed() && CanTheGunFire()) {
				firingRoutine = StartCoroutine(FireMachineGun());
			} else if (!ShootButtonPressed() && IsTheGunCurrentlyFiring()) {
				StopFiring();
			}
		}

		public void StopFiring() {
			UpdateGunStateToReloading();
			StopCoroutine(firingRoutine);
			StartCoroutine(CoolDown()); // todo: Cool down should only be needed once max fireable bullets reached
		}

		IEnumerator FireMachineGun() {
			UpdateGunStateToFiring();

			foreach (var i in System.Linq.Enumerable.Range(0, maxFirableMachineGunBullets))
			{
				var spawnedProjectile = SimplePool.Spawn(projectile, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
				SimplePool.Despawn(spawnedProjectile, .025f);
				bulletsFiredCounter++;
				yield return new WaitForSeconds(.025f);
			}

			StopFiring();
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

		void ResetBulletsFiredCounter() {
			bulletsFiredCounter = 0;
		}

		IEnumerator CoolDown() {
			gunState.state = GunState.State.Reloading;
			yield return new WaitForSeconds((bulletsFiredCounter / 10) * coolDownMultiplier);
			ResetBulletsFiredCounter();
			gunState.state = GunState.State.Ready;
		}
	}
}
