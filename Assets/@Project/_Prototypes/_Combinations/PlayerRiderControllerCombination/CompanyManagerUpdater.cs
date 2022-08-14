using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderTabletSystem;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.State;

namespace DuneRiders.PlayerRiderControllerCombination {
	[RequireComponent(typeof(CompanyManagementDisplayController))]
	public class CompanyManagerUpdater : MonoBehaviour
	{
		CompanyManagementDisplayController companyManager;

		void Awake() {
			companyManager = GetComponent<CompanyManagementDisplayController>();
		}

		void OnEnable() {
			StartCoroutine(UpdateRiderList());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		IEnumerator UpdateRiderList() {
			while (true) {
				var allCompanyRiders = GetAllCompanyRiders();
				companyManager.ridersToDisplay = RidersToRiderDisplayObjects(allCompanyRiders);

				yield return new WaitForSeconds(1f);
			}
		}

		Rider[] GetAllCompanyRiders() {
			var allRiders = FindObjectsOfType<Rider>();

			return allRiders.Where((rider) => {
				if (rider.allegiance == AI.Allegiance.Player && rider.GetComponent<Player>() == null) return true;
				return false;
			}).ToArray();
		}

		List<CompanyManagementDisplayController.RiderToDisplay> RidersToRiderDisplayObjects(Rider[] riders) {
			var riderDisplayList = new List<CompanyManagementDisplayController.RiderToDisplay>();

			for (int i = 0; i < riders.Length; i++) {
				riderDisplayList.Add(new CompanyManagementDisplayController.RiderToDisplay {
					armourType = ArmourTypeEnumConverter(riders[i].chasisType),
					weaponType = WeaponTypeEnumConverter(riders[i].gunType),
					health = riders[i].GetComponent<HealthState>().health,
					// DisbandCallback disbandCallback;
				});
			}

			return riderDisplayList;
		}

		CompanyManagementDisplayController.ArmourType ArmourTypeEnumConverter(Rider.ChasisType chasis) {
			switch (chasis) {
				case Rider.ChasisType.Heavy:
					return CompanyManagementDisplayController.ArmourType.Heavy;
				case Rider.ChasisType.Normal:
					return CompanyManagementDisplayController.ArmourType.Medium;
				case Rider.ChasisType.Light:
					return CompanyManagementDisplayController.ArmourType.Light;
				default:
					return CompanyManagementDisplayController.ArmourType.Light;
			}
		}

		CompanyManagementDisplayController.WeaponType WeaponTypeEnumConverter(Rider.GunType gunType) {
			switch (gunType) {
				case Rider.GunType.Cannon:
					return CompanyManagementDisplayController.WeaponType.Cannon;
				case Rider.GunType.TripleMissileLauncher:
					return CompanyManagementDisplayController.WeaponType.TripleMissileLauncher;
				case Rider.GunType.MachineGun:
					return CompanyManagementDisplayController.WeaponType.MachineGun;
				default:
					return CompanyManagementDisplayController.WeaponType.MachineGun;
			}
		}
	}
}
