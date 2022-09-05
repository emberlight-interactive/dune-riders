using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.Actioners;

namespace DuneRiders.RiderAICombination {
	[RequireComponent(typeof(Rider))]
	[RequireComponent(typeof(Gunner))]
	public class TurretInitializer : MonoBehaviour
	{
		Rider rider;
		Gunner gunner;
		[SerializeField] Turret cannon;
		[SerializeField] Turret tripleMissileLauncher;
		[SerializeField] Turret machineGun;

		void Awake() {
			rider = GetComponent<Rider>();
			gunner = GetComponent<Gunner>();

			if (rider.gunType == Rider.GunType.Cannon) {
				ActivateAndAssignTurretToGunner(cannon);
			} else if (rider.gunType == Rider.GunType.TripleMissileLauncher) {
				ActivateAndAssignTurretToGunner(tripleMissileLauncher);
			} else if (rider.gunType == Rider.GunType.MachineGun) {
				ActivateAndAssignTurretToGunner(machineGun);
			}
		}

		void ActivateAndAssignTurretToGunner(Turret turret) {
			turret.gameObject.SetActive(true);
			gunner.turret = turret;
		}
	}
}
