using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace DuneRiders.Prototype
{
	public class VehicleReOrientation : MonoBehaviour
	{
		[BoxGroup("Variables"), SerializeField] private float reOrientHeight = 5;
		[BoxGroup("Variables"), SerializeField] private float reOrientSpeed = 1.5f;
		[BoxGroup("Variables"), SerializeField] private float timeBeforeUnstuck = 2f;

		[BoxGroup("Components"), SerializeField] private BoxCollider stuckTrigger;

		private float unstuckCount = 0;

		private void TriggerUnstuck()
		{
			StartCoroutine(UnStuck());
		}

		private IEnumerator UnStuck()
		{
			//Remove player control

			Vector3 dest = new Vector3(transform.position.x, transform.position.y + reOrientHeight, transform.position.z);
			Vector3 rot = new Vector3(0, transform.rotation.y, 0);

			transform.position = dest;
			transform.rotation = Quaternion.Euler(rot);
			yield return new WaitForSeconds(reOrientSpeed);

			//Re enable controls
		}

		private void OntriggerEnter(Collider c)
		{
			if (c.tag == "Ground")
			{
				unstuckCount = 0;
			}
		}

		private void OnTriggerStay(Collider c)
		{
			if (c.tag == "Ground")
			{
				unstuckCount += Time.deltaTime;
				if (unstuckCount > timeBeforeUnstuck)
				{
					unstuckCount = 0;
					StartCoroutine(UnStuck());
				}
			}
		}

		private void OnTriggerExit(Collider c)
		{
			unstuckCount = 0;
		}
	}
}