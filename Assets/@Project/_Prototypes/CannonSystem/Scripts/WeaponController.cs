using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

namespace DuneRiders.Prototype
{
	public class WeaponController : MonoBehaviour
	{
		[BoxGroup("Debug"), SerializeField] private bool isDebug = true;
		[BoxGroup("Debug"), ShowIf("isDebug", true), SerializeField] private Color forwardLineColor = Color.green;

		[BoxGroup("Weapons & Variables"), SerializeField] private List<BaseWeapon> weapons = new List<BaseWeapon>();
		[BoxGroup("Weapons & Variables"), SerializeField] private float weaponChangePause = .5f;
		[BoxGroup("Weapons & Variables"), SerializeField] private float aimSensetivity;

		[BoxGroup("Input Actions"), SerializeField] private InputActionProperty aimInput;
		[BoxGroup("Input Actions"), SerializeField] private InputActionProperty nextWeaponInput;
		[BoxGroup("Input Actions"), SerializeField] private InputActionProperty shootInput;

		private BaseWeapon currentWeapon = null;
		private int weaponIndex = 0;
		private bool weaponChanging = false;
		private bool weaponActive = false;

		private void Start()
		{
			aimInput.action.Enable();
			nextWeaponInput.action.Enable();
			shootInput.action.Enable();
		}

		//TODO maybe add a threshhold on next weapon click, swap weapons if under X time, otherwise put away completely.

		private void Update()
		{
			if (weaponActive)
			{
				var input = aimInput.action.ReadValue<Vector2>();
				transform.Rotate(input * (aimSensetivity * Time.deltaTime));
			}

			if (nextWeaponInput.action.ReadValue<float>() > 0 && weaponChanging == false)
				NextWeapon();

			if (shootInput.action.ReadValue<float>() > 0)
				Shoot();
		}

		public void Shoot()
		{
			if (weaponChanging == false)
				weapons[weaponIndex].Shoot();
		}

		public void SwitchToWeapon(int index)
		{
			if (weaponIndex - 1 <= weapons.Count)
				StartCoroutine(EnableWeapon(index));
		}

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
			weaponActive = false;
			weaponChanging = true;

			if (currentWeapon != null)
			{
				weapons[weaponIndex].DeActivate();
				yield return new WaitForSeconds(weapons[weaponIndex].GetDeActivationTime());
			}

			weaponActive = true;
			weaponIndex = next;
			weapons[weaponIndex].Activate();
			yield return new WaitForSeconds(weapons[weaponIndex].GetActivationTime());
			currentWeapon = weapons[next];
			weaponChanging = false;
		}

		private void OnDrawGizmos()
		{
			if (isDebug)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(transform.position, transform.forward * 100);
			}
		}
	}
}