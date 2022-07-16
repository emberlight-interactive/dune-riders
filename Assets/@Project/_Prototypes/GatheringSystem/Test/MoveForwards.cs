using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Test {
	public class MoveForwards : MonoBehaviour
	{
		public float movementSpeed = 1;

		void Update()
		{
			transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
		}
	}
}
