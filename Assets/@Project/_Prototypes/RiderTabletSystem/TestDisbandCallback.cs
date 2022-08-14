using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderTabletSystem {
	public class TestDisbandCallback : MonoBehaviour
	{
		[SerializeField] CompanyManagementDisplayController companyManager;

		public void Disband() {
			Debug.Log("Disband: " + gameObject.name);
		}

		void Start() {
			companyManager.ridersToDisplay = new List<CompanyManagementDisplayController.RiderToDisplay>() {
				new CompanyManagementDisplayController.RiderToDisplay {
					armourType = CompanyManagementDisplayController.ArmourType.Heavy,
					weaponType = CompanyManagementDisplayController.WeaponType.MachineGun,
					health = 75,
					disbandCallback = Disband,
				}
			};
		}
	}
}
