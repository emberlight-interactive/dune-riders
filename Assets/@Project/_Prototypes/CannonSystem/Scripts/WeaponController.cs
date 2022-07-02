using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.Prototype
{
	public class WeaponController : MonoBehaviour
	{
		[BoxGroup("Debug"), SerializeField] private bool isDebug = true;

		[BoxGroup("Weapons & Variables"), SerializeField] private List<BaseWeapon> weapons = new List<BaseWeapon>();
		//? When the weapon changes how long does it wait before activating the next weapon?
		[BoxGroup("Weapons & Variables"), SerializeField] private float weaponChangePause = .5f;

		private BaseWeapon currentWeapon = null;
		private int weaponIndex = 0;
		private bool weaponChanging = false;

		//! Delete after testing
		private bool debugAutoShoot = false;

		private void Start()
		{
			StartCoroutine(EnableWeapon(0));
		}

		private void Update()
		{
			if (debugAutoShoot)
				weapons[weaponIndex].Shoot();
		}

		[Button]
		public void ToggleShootingTest()
		{
			debugAutoShoot = !debugAutoShoot;
		}

		[Button]
		public void NextWeapon()
		{
			if (weaponChanging)
				return;

			int nextWeapon = 0;

			if (weaponIndex + 1 <= weapons.Count - 1)
				nextWeapon = weaponIndex + 1;
			else
				nextWeapon = 0;

			StartCoroutine(EnableWeapon(nextWeapon));
		}

		[Button]
		public void PreviousWeapon()
		{
			if (weaponChanging)
				return;

			int nextWeaponIndex = 0;

			if (weaponIndex - 1 >= 0)
				nextWeaponIndex = weaponIndex - 1;
			else
				nextWeaponIndex = weapons.Count - 1;

			StartCoroutine(EnableWeapon(nextWeaponIndex));
		}

		private IEnumerator EnableWeapon(int next)
		{
			if (currentWeapon != null)
			{
				if (isDebug)
					Debug.Log("Changing weapons");
				weaponChanging = true;
				weapons[weaponIndex].DeActivate();
				yield return new WaitForSeconds(weapons[weaponIndex].GetDeActivationTime());
			}

			weaponIndex = next;
			weapons[weaponIndex].Activate();
			yield return new WaitForSeconds(weapons[weaponIndex].GetActivationTime());
			currentWeapon = weapons[next];
			weaponChanging = false;
			if (isDebug)
				Debug.Log("Weapon changed to " + weapons[weaponIndex].GetWeaponName());
		}
	}
	//Not going backwards
}