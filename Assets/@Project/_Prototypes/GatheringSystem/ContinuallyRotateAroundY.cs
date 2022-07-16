using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuallyRotateAroundY : MonoBehaviour
{
    public float rotationSpeed = 40;

	void Update() {
		transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
	}
}
