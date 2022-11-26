using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour
{
	[SerializeField] Camera cam;
	[SerializeField] bool zoom = false;
	[SerializeField] float zoomRate = 1f;

	[SerializeField] bool pan = true;
	[SerializeField] float panSpeed = 1f;
	[SerializeField] Axis axisToPan = Axis.Up;

	[SerializeField] bool rotateTarget = false;
	[SerializeField] Transform targetToRotate;

	[SerializeField] bool dolly = false;
	[SerializeField] Vector3 movement = Vector3.one;

	enum Axis {Up, Right, Forward};
    // Update is called once per frame
    void Update()
    {
		if (pan) Pan();
		else if (rotateTarget) RotateTarget();
		else if (dolly) Dolly();

		if (zoom) Zoom();
    }

	Vector3 GetAxis(Transform targetTransform = null) {
		var tTransform = targetTransform ?? transform;

		switch (axisToPan) {
			case Axis.Up: return tTransform.up;
			case Axis.Right: return tTransform.right;
			case Axis.Forward: return tTransform.forward;
			default: return tTransform.up;
		}
	}

	void Pan() {
		var totalRotation = (panSpeed * Time.deltaTime);
        transform.RotateAround(transform.position, GetAxis(), totalRotation);
	}

	void RotateTarget() {
		var totalRotation = (panSpeed * Time.deltaTime);
        transform.RotateAround(targetToRotate.position, GetAxis(targetToRotate), totalRotation);
		transform.LookAt(targetToRotate);
	}

	void Dolly() {
		transform.localPosition = transform.localPosition += movement * Time.deltaTime;
	}

	void Zoom() {
		cam.fieldOfView -= zoomRate * Time.deltaTime;
	}
}
