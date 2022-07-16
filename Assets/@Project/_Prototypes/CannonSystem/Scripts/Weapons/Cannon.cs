using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.Prototype
{
	public class Cannon : BaseWeapon
	{
		[BoxGroup("Gun Effects"), SerializeField] private ParticleSystem muzzleFlash;

		public override void Shoot()
		{
			base.Shoot();
		}

		public override void PlayGunEffect()
		{
			base.PlayGunEffect();

			muzzleFlash.Play();
		}
	}
}
