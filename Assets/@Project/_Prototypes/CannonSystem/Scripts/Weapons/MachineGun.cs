using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.Prototype
{
	public class MachineGun : BaseWeapon
	{
		[BoxGroup("Components"), SerializeField] private LineRenderer bulletLine;

		[BoxGroup("Gun Effects"), SerializeField] private float lineDisplayDuration = .1f;
		[BoxGroup("Gun Effects"), SerializeField] private ParticleSystem muzzleFlash;
		[BoxGroup("Gun Effects"), SerializeField] private ParticleSystem bulletImpact;

		private float lastLineDisplayTime = 999;
		private float currentLineDisplayTime = 0;


		public override void Shoot()
		{
			base.Shoot();
		}

		public override void PlayGunEffect()
		{
			base.PlayGunEffect();

			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
			{
				muzzleFlash.Play();
				bulletImpact.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z - .25f);
				bulletImpact.Play();

				lastLineDisplayTime = Time.time;
				bulletLine.positionCount = 2;
				bulletLine.SetPosition(0, projectileOrigin.position);
				bulletLine.SetPosition(1, hit.point);
			}
		}

		private void Update()
		{
			if (bulletLine.positionCount > 0)
			{
				currentLineDisplayTime += Time.deltaTime;

				if (currentLineDisplayTime > lineDisplayDuration)
				{
					bulletLine.positionCount = 0;
					currentLineDisplayTime = 0;
				}
			}
		}

		public override void Reload()
		{
			base.Reload();
		}
	}
}

