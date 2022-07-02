using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.Prototype
{
	public class MachineGun : BaseWeapon
	{
		public Transform bulletOrigin;

		public override void Shoot()
		{
			base.Shoot();

			Instantiate(projectile, bulletOrigin.position, bulletOrigin.rotation);
		}

		public override void Reload()
		{
			base.Reload();
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(bulletOrigin.position, .05f);
		}
	}
}
