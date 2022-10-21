using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI {
	public class ISpinWhenIMoveHaha : MonoBehaviour
	{
		Vector3 currPos = Vector3.zero;
		[SerializeField] float spinSpeed = 4800f;

		void Update() {
			if (transform.position != currPos) {
				transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime * Vector3.Distance(transform.position, currPos)), Space.Self);
				currPos = transform.position;
			}
		}
	}
}
