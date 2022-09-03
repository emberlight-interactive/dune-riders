using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI {
	public class TurretPrefabAttach : MonoBehaviour
	{
		[SerializeField] GameObject cannon;
		[SerializeField] GameObject tripleMissileLauncher;
		[SerializeField] GameObject machineGun;
		Rider parentContextSource;

		void Awake() {
			InitParentContextSource();
			InstantiateTurretHere();
		}

		void InitParentContextSource() {
			parentContextSource = GetComponentInParent<Rider>();
		}

		void InstantiateTurretHere() {
			switch (parentContextSource.gunType) {
				case Rider.GunType.Cannon:
					Instantiate(cannon, transform);
					break;
				case Rider.GunType.TripleMissileLauncher:
					Instantiate(tripleMissileLauncher, transform);
					break;
				case Rider.GunType.MachineGun:
					Instantiate(machineGun, transform);
					break;
			}
		}
	}
}
