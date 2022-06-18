using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Prototype
{
	public interface IWeapon
	{
		public void Shoot();
		public void Reload();
		public float GetCooldown();
	}
}
