using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Prototype
{
	public interface IWeapon
	{
		public string GetWeaponName();
		public void Shoot();
		public void Reload();
		public void Activate();
		public void DeActivate();
		public float GetCooldown();
		public float GetActivationTime();
		public float GetDeActivationTime();
		public float GetRange();
	}
}
