using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Prototype
{
	public class WeaponController : MonoBehaviour
	{
		[SerializeField] private List<BaseWeapon> weapons = new List<BaseWeapon>();
		[SerializeField] private float weaponChangeTime = .5f;

		private BaseWeapon currentWeapon;
		private int weaponIndex = 0;
		private bool weaponChanging = false;

		private void Start()
		{
			currentWeapon = weapons[0];

			StartCoroutine(EnableWeapon(currentWeapon));
		}

		public void NextWeapon()
		{
			if (weaponChanging)
				return;

			if (weaponIndex + 1 <= weapons.Count - 1)
				weaponIndex++;
			else
				weaponIndex = 0;
		}

		public void PreviousWeapon()
		{
			if (weaponChanging)
				return;

			if (weaponIndex - 1 >= 0)
				weaponIndex--;
			else
				weaponIndex = weapons.Count - 1;

			print(weaponIndex);
		}

		private IEnumerator EnableWeapon(BaseWeapon weapon)
		{
			weaponChanging = true;
			yield return new WaitForSeconds(weaponChangeTime);
			weaponChanging = false;
		}
	}
}