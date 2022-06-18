using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Prototype
{
	public class BaseWeapon : MonoBehaviour, IWeapon
	{
		[SerializeField] private float cooldown;
		[SerializeField] private float range;
		[SerializeField] private AnimationCurve damageCurve;

		public virtual void Shoot()
		{

		}

		public virtual void Reload()
		{

		}

		public virtual float GetCooldown()
		{
			return cooldown;
		}

		public virtual float GetRange()
		{
			return range;
		}
	}
}