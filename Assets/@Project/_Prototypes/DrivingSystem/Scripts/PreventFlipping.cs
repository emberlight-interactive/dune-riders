using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventFlipping : MonoBehaviour
{
    float xRotationLimit = 20;
	float zRotationLimit = 20;

	void Update () {
		transform.localEulerAngles = new Vector3(
			ClampAngle(transform.localEulerAngles.x, -xRotationLimit, xRotationLimit),
			transform.localEulerAngles.y,
			ClampAngle(transform.localEulerAngles.z, -zRotationLimit, zRotationLimit)
		);
	}

	float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if(min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }

        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }
}
