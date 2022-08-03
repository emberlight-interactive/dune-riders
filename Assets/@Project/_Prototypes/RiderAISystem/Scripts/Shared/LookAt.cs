using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI.Shared {
	public class LookAt : MonoBehaviour
	{
		[SerializeField] Transform target;
		[SerializeField] bool onlyPivotAroundY = true;
		Vector3 originalRotation;

		void Start() {
			if (target == null) target = Camera.main.transform;
			originalRotation = transform.eulerAngles;
		}

		void Update()
		{
			transform.LookAt(target);

			if (onlyPivotAroundY) {
				var newRotation = new Vector3(originalRotation.x, transform.rotation.eulerAngles.y, originalRotation.z);
				transform.eulerAngles = newRotation;
			}
		}
	}
}
