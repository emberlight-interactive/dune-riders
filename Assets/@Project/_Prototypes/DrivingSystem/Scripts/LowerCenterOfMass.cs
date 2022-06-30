using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LowerCenterOfMass : MonoBehaviour
{
	public float lowerAmount;

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass += new Vector3(0, lowerAmount, 0);
    }
}
