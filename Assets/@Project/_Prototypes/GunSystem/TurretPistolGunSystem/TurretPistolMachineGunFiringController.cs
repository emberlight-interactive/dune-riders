using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class TurretPistolMachineGunFiringController : MonoBehaviour
	{
		[SerializeField] float coolDownMultiplier = 1f;
		[SerializeField] int maxFirableMachineGunBullets = 20;
		[SerializeField] Transform projectileSpawnLocation;
		[SerializeField] GameObject projectile;
		Coroutine firingRoutine;
		Coroutine coolDownRoutine;
		GunState gunState;
		public int bulletsFiredCounter = 0;
		bool triggerPulled = false;

		void Awake() {
			gunState = GetComponent<GunState>();
		}

		void OnEnable() {
			if (coolDownRoutine != null) {
				StopCoroutine(coolDownRoutine);
				coolDownRoutine = null;
			}

			coolDownRoutine = StartCoroutine(CoolDownSystem());
		}

		void Update() {
			if (ShootButtonPressed() && CanTheGunFire()) {
				firingRoutine = StartCoroutine(FireMachineGun());
			} else if (!ShootButtonPressed() && IsTheGunCurrentlyFiring()) {
				StopFiring();
			}
		}

		public void StopFiring() {
			StopCoroutine(firingRoutine);
			UpdateGunStateToReloading();
		}

		IEnumerator FireMachineGun() {
			UpdateGunStateToFiring();

			foreach (var i in System.Linq.Enumerable.Range(0, (maxFirableMachineGunBullets + 1) - bulletsFiredCounter))
			{
				bulletsFiredCounter++;
				var spawnedProjectile = SimplePool.Spawn(projectile, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
				SimplePool.Despawn(spawnedProjectile, .025f);
				yield return new WaitForSeconds(.025f);
			}

			StopFiring();
		}

		bool ShootButtonPressed() {
			return triggerPulled;
		}

		public void ToggleTriggerPull() {
			triggerPulled = !triggerPulled;
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

		bool IsFullCoolDownTriggered() {
			return bulletsFiredCounter >= maxFirableMachineGunBullets;
		}

		IEnumerator CoolDownSystem() {
			bool isTheGunCurrentlyInCoolDown = false;
			while (true) {
				while (!IsTheGunCurrentlyFiring()) {
					if (IsFullCoolDownTriggered()) {
						gunState.state = GunState.State.Reloading;
						isTheGunCurrentlyInCoolDown	= true;
					} else if (isTheGunCurrentlyInCoolDown && bulletsFiredCounter == 0) {
						gunState.state = GunState.State.Ready;
						isTheGunCurrentlyInCoolDown	= false;
					} else if (!isTheGunCurrentlyInCoolDown && gunState.state == GunState.State.Reloading) {
						gunState.state = GunState.State.Ready;
					}

					if (bulletsFiredCounter > 0) {
						yield return new WaitForSeconds(0.075f * coolDownMultiplier);
						bulletsFiredCounter--;
					} else {
						yield return null;
					}
				}

				yield return null;
			}
		}
	}
}
