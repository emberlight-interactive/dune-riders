using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.Prototype
{
	public class Rockets : BaseWeapon
	{
		[BoxGroup("Gun Effects"), SerializeField] private ParticleSystem[] muzzleFlash;

		public override void Shoot()
		{
			base.Shoot();
		}

		public override void PlayGunEffect()
		{
			base.PlayGunEffect();

			for (int i = 0; i < muzzleFlash.Length; i++)
				muzzleFlash[i].Play();
		}
	}
}
