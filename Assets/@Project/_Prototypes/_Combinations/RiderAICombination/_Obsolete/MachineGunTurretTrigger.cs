using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;

namespace DuneRiders.RiderAICombination {
	[System.Obsolete()]
	public class MachineGunTurretTrigger : TurretTrigger
	{
		[SerializeField] float coolDownMultiplier = 1f;
		[SerializeField] int maxFirableMachineGunBullets = 20;
		[SerializeField] Transform projectileSpawnLocation;
		[SerializeField] GameObject projectile;
		bool firing = false;
		bool reloading = false;
		int bulletsFiredCounter = 0;

		void Start() {
			StartCoroutine(CoolDownSystem());
		}

		public override void PullTrigger() {
			if (!reloading && !firing) {
				StartCoroutine(FireMachineGunBullet());
				firing = true;
			}
		}

		public override void ReleaseTrigger() {}

		bool IsFullCoolDownTriggered() {
			return bulletsFiredCounter >= maxFirableMachineGunBullets;
		}

		IEnumerator FireMachineGunBullet() {
			firing = true;
			bulletsFiredCounter++;
			var spawnedProjectile = SimplePool.Spawn(projectile, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
			SimplePool.Despawn(spawnedProjectile, .025f);
			yield return new WaitForSeconds(.025f);
			firing = false;
		}

		IEnumerator CoolDownSystem() {
			bool isTheGunCurrentlyInCoolDown = false;
			while (true) {
				while (!firing || IsFullCoolDownTriggered()) {
					if (IsFullCoolDownTriggered()) {
						reloading = true;
						isTheGunCurrentlyInCoolDown	= true;
					} else if (isTheGunCurrentlyInCoolDown && bulletsFiredCounter == 0) {
						reloading = false;
						isTheGunCurrentlyInCoolDown	= false;
					} else if (!isTheGunCurrentlyInCoolDown && reloading) {
						reloading = false;
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
