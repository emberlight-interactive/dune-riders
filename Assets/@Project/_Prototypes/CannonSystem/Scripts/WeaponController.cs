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

		[BoxGroup("Components"), SerializeField] private GameObject weaponContainer;
		[BoxGroup("Components"), SerializeField] private Transform weaponBase;
		[BoxGroup("Components"), SerializeField] private Transform weaponPivot;

		[BoxGroup("Weapons & Variables"), SerializeField] private List<BaseWeapon> weapons = new List<BaseWeapon>();
		[BoxGroup("Weapons & Variables"), SerializeField] private float aimSensetivity;
		[BoxGroup("Weapons & Variables"), SerializeField] private int baseAngle = 45;
		[BoxGroup("Weapons & Variables"), SerializeField] private int pivotAngle = 45;
		[BoxGroup("Weapons & Variables"), InfoBox("If the nextWeapon input is held longer than this, it will disable the weapon."), SerializeField] private float weaponChangeThreshold;

		[BoxGroup("Input Actions"), SerializeField] private InputActionProperty aimInput;
		[BoxGroup("Input Actions"), SerializeField] private InputActionProperty nextWeaponInput;
		[BoxGroup("Input Actions"), SerializeField] private InputActionProperty shootInput;

		private BaseWeapon currentWeapon = null;
		private int weaponIndex = 0;
		private bool weaponActive = false; //? change this after the play back is changed.

		//weapon change input
		private bool weaponChanging = false;
		private float weaponChangeCurrentTick = 0;

		private bool autoShoot = false;

		private void Start()
		{
			aimInput.action.Enable();
			nextWeaponInput.action.Enable();
			shootInput.action.Enable();
		}

		private void Update()
		{
			if (autoShoot)
				Shoot();

			if (nextWeaponInput.action.ReadValue<float>() > 0)
			{
				if (weaponActive == false)
				{
					NextWeapon();
					weaponActive = true;
				}
				weaponChangeCurrentTick += Time.deltaTime;
			}
			else if (weaponChangeCurrentTick > 0)
			{
				if (weaponActive && weaponChangeCurrentTick < weaponChangeThreshold)
					NextWeapon();
				else
				{
					if (weaponChanging == false)
						StartCoroutine(DeActivateWeapon());

					weaponChangeCurrentTick = 0;
					weaponActive = false;
				}

				weaponChangeCurrentTick = 0;
			}

			if (weaponActive == false)
				return;

			if (aimInput.action.ReadValue<Vector2>() != Vector2.zero)
			{
				var xRot = aimInput.action.ReadValue<Vector2>().x;
				var rotateAmount = xRot * (aimSensetivity * Time.deltaTime);

				Vector3 currentRotation = weaponBase.transform.localEulerAngles;
				currentRotation.y = currentRotation.y % 360;

				if (currentRotation.y > 180)
					currentRotation.y -= 360f;

				currentRotation.y = Mathf.Clamp(currentRotation.y + rotateAmount, -baseAngle / 2, baseAngle / 2);
				weaponBase.transform.localRotation = Quaternion.Euler(currentRotation);

				var yRot = aimInput.action.ReadValue<Vector2>().y;
				var pivotRotAmount = yRot * (aimSensetivity * Time.deltaTime);

				currentRotation = weaponPivot.transform.localEulerAngles;
				currentRotation.x = currentRotation.x % 360;

				if (currentRotation.x > 180)
					currentRotation.x -= 360f;

				currentRotation.x = Mathf.Clamp(currentRotation.x - pivotRotAmount, -pivotAngle / 2, pivotAngle / 2);
				weaponPivot.transform.localRotation = Quaternion.Euler(currentRotation);

			}

			if (weaponChanging == false && shootInput.action.ReadValue<float>() > 0)
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

		[Button]
		public void AutoShoot()
		{
			autoShoot = true;
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
				weaponPivot.transform.localRotation = Quaternion.Euler(Vector3.zero);
				yield return new WaitForSeconds(weapons[weaponIndex].GetDeActivationTime());
			}

			weaponIndex = next;
			weapons[weaponIndex].Activate();
			yield return new WaitForSeconds(weapons[weaponIndex].GetActivationTime());
			currentWeapon = weapons[next];
			weaponChanging = false;
			weaponActive = true;
		}

		private IEnumerator DeActivateWeapon()
		{
			weaponChanging = true;
			weaponPivot.transform.localRotation = Quaternion.Euler(Vector3.zero);
			weapons[weaponIndex].DeActivate();
			yield return new WaitForSeconds(weapons[weaponIndex].GetDeActivationTime());
			weaponActive = false;
			weaponActive = false;
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
