using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAICombination {
	[RequireComponent(typeof(Rider))]
	public class TurretActivator : MonoBehaviour
	{
		Rider rider;
		[SerializeField] GameObject cannon;
		[SerializeField] GameObject tripleMissileLauncher;
		[SerializeField] GameObject machineGun;

		void Awake() {
			rider = GetComponent<Rider>();

			if (rider.gunType == Rider.GunType.Cannon) {
				cannon.SetActive(true);
			} else if (rider.gunType == Rider.GunType.TripleMissileLauncher) {
				tripleMissileLauncher.SetActive(true);
			} else if (rider.gunType == Rider.GunType.MachineGun) {
				machineGun.SetActive(true);
			}
		}
	}
}
