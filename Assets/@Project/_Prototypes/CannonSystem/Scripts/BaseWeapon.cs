using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.Prototype
{
	public class BaseWeapon : MonoBehaviour, IWeapon
	{
		[BoxGroup("Debug"), SerializeField] private bool isDebug = true;
		[BoxGroup("Debug"), ShowIf("isDebug", true), SerializeField] private Color rangeColor;

		[BoxGroup("Weapon Stats"), SerializeField] private string weaponName;
		[BoxGroup("Weapon Stats"), SerializeField] private float cooldown;
		[BoxGroup("Weapon Stats"), InfoBox("The activation and deactivation times should be equal to or greater than the animation time."), SerializeField] private float activationTime;
		[BoxGroup("Weapon Stats"), SerializeField] private float deactivationTime;
		[BoxGroup("Weapon Stats"), SerializeField] private int ammoCount;
		[BoxGroup("Weapon Stats"), SerializeField] private int currentAmmo;
		[BoxGroup("Weapon Stats"), SerializeField] private bool autoReload = true;
		[BoxGroup("Weapon Stats"), SerializeField] private float reloadTime = 2.0f;
		[BoxGroup("Weapon Stats"), SerializeField] private float range;
		[BoxGroup("Weapon Stats"), SerializeField] internal BaseProjectile projectile;

		[BoxGroup("Components"), SerializeField] internal Transform projectileOrigin;
		[BoxGroup("Components"), SerializeField] private Animator anim;
		[BoxGroup("Components"), SerializeField] private List<MeshRenderer> meshes;

		private float lastShotTime = 0;
		private bool reloading = false;
		// private bool activated = false;

		private void Start()
		{
			currentAmmo = ammoCount;

			meshes.ForEach((x) => x.enabled = false);
		}

		public virtual void Shoot()
		{
			if (currentAmmo > 0 && Time.time - lastShotTime > cooldown)
			{
				if (isDebug)
					Debug.Log("Shot " + weaponName);

				lastShotTime = Time.time;
				currentAmmo--;
				Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
				PlayGunEffect();
			}
			else if (currentAmmo == 0 && autoReload && reloading == false)
			{
				Reload();
			}

			//TODO Add animation trigger.
			//TODO Add sound trigger.
			//TODO Update ammo UI.
		}

		public virtual void Reload()
		{
			if (reloading == false)
				StartCoroutine(ReloadRoutine());

			//TODO Add animation trigger.
			//TODO Add sound trigger.
			//TODO Show ui
		}

		public void Activate()
		{
			if (isDebug)
				Debug.Log("BaseWeapon::Activate " + weaponName);

			meshes.ForEach((x) => x.enabled = true);
			anim.SetTrigger("activate");
		}

		public void DeActivate()
		{
			if (isDebug)
				Debug.Log("BaseWeapon::DeActivate " + weaponName);

			anim.SetTrigger("deactivate");
			Invoke("DisableMesh", GetDeActivationTime());
		}

		private void DisableMesh()
		{
			meshes.ForEach((x) => x.enabled = false);
		}

		public virtual void PlayGunEffect()
		{

		}

		public string GetWeaponName()
		{
			return weaponName;
		}

		public float GetCooldown()
		{
			return cooldown;
		}

		public float GetRange()
		{
			return range;
		}

		public float GetActivationTime()
		{
			return activationTime;
		}

		public float GetDeActivationTime()
		{
			return deactivationTime;
		}

		private IEnumerator ReloadRoutine()
		{
			reloading = true;
			yield return new WaitForSeconds(reloadTime);
			currentAmmo = ammoCount;
			reloading = false;

			Debug.Log("reload complete");
		}

		private void OnDrawGizmos()
		{
			if (isDebug == false)
				return;

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(projectileOrigin.position, .05f);

			Gizmos.color = rangeColor;
			Gizmos.DrawSphere(transform.position, range);
		}
	}
}
