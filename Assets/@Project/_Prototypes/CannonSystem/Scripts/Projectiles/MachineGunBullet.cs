using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Prototype
{
	public class MachineGunBullet : BaseProjectile
	{
		private void Start()
		{
			RayCastTarget();
		}

		private void RayCastTarget()
		{
			RaycastHit hit;

			Debug.DrawRay(transform.position, transform.forward * 100, Color.red);

			if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
			{
				Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
                
				Debug.Log("Did Hit");
			}

			Destroy(this);
		}
	}
}
