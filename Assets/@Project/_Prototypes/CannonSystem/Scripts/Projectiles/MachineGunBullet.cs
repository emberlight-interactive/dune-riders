using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.Prototype
{
	public class MachineGunBullet : BaseProjectile
	{
		[BoxGroup("Debug"), SerializeField] private bool isDebug = true;
		[BoxGroup("Debug"), ShowIf("isDebug", true), SerializeField] private Color bulletHitColor = Color.red;
		[BoxGroup("Debug"), ShowIf("isDebug", true), SerializeField] private Color bulletLineColor = Color.yellow;

		private void Start()
		{
			RayCastTarget();
		}

		private void RayCastTarget()
		{
			RaycastHit hit;

			if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
			{
				if (isDebug)
					Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);

				if (hit.collider.GetComponent<TestTarget>() != null)
					hit.collider.GetComponent<TestTarget>().OnHit();
			}
			else
			{
				if (isDebug)
					Debug.DrawRay(transform.position, transform.forward * 100, bulletLineColor);
			}

			Destroy(gameObject);
		}
	}
}
