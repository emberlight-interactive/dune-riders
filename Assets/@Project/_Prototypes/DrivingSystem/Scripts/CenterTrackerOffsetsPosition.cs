using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTrackerOffsetsPosition : MonoBehaviour
{
	void Start()
    {
        StartCoroutine(Center());
    }

	IEnumerator Center() {
		yield return new WaitForSeconds(0.4f);
		while (transform.localPosition != Vector3.zero) {
			transform.localPosition = Vector3.zero;
			yield return new WaitForSeconds(0.1f);
		}
	}
}
