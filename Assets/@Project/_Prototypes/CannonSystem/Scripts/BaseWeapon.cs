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
		[BoxGroup("Weapon Stats"), SerializeField] private float ammoCount;
		[BoxGroup("Weapon Stats"), SerializeField] private float range;
		[BoxGroup("Weapon Stats"), SerializeField] private BaseProjectile projectile;

		[BoxGroup("Components"), SerializeField] private Animator anim;

		private float lastShotTime = 0;
		private bool activated = false;

		public virtual void Shoot()
		{
			if (Time.time - lastShotTime > cooldown)
			{
				lastShotTime = Time.time;
				print("Instance a projectile from " + weaponName);
			}

			//TODO Add animation trigger.
			//TODO Add sound trigger.
			//TODO Update ammo UI.
		}

		public virtual void Reload()
		{
			//TODO Add animation trigger.
			//TODO Add sound trigger.
			//TODO Show ui 
		}

		public void Activate()
		{
			if (isDebug)
				Debug.Log("BaseWeapon::Activate " + weaponName);
		}

		public void DeActivate()
		{
			if (isDebug)
				Debug.Log("BaseWeapon::DeActivate " + weaponName);
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

		private void OnDrawGizmos()
		{
			if (isDebug == false)
				return;

			Gizmos.color = rangeColor;
			Gizmos.DrawSphere(transform.position, range);
		}
	}
}