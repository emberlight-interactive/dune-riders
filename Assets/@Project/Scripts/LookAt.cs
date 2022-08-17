using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	public class LookAt : MonoBehaviour
	{
		public Transform target;
		[SerializeField] bool onlyPivotAroundY = true;
		[SerializeField] bool gradual = false;
		[SerializeField] float gradualStep = 1.0f;
		Vector3 originalRotation;

		void Start() {
			if (target == null) target = Camera.main.transform;
			originalRotation = transform.eulerAngles;
		}

		void Update()
		{
			if (gradual) {
				Vector3 relativePos = target.position - transform.position;
				Quaternion toRotation = Quaternion.LookRotation(relativePos);
				transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, gradualStep * Time.deltaTime);
			} else {
				transform.LookAt(target);
			}

			if (onlyPivotAroundY) {
				var newRotation = new Vector3(originalRotation.x, transform.rotation.eulerAngles.y, originalRotation.z);
				transform.eulerAngles = newRotation;
			}
		}
	}
}
