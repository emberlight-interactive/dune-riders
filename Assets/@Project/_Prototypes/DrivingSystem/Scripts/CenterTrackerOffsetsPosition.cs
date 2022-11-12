using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTrackerOffsetsPosition : MonoBehaviour
{
	[SerializeField] bool setHeightWhenBelowThreshold = false;
	[SerializeField] Transform cam;
	[SerializeField] Transform camOffset;
	[SerializeField] Transform camFollowerOffset;

	[SerializeField] float heightMinimumThreshold;
	[SerializeField] float heightToSet;

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

		if (setHeightWhenBelowThreshold) {
			if (cam.localPosition.y <= heightMinimumThreshold) {
				var camYOffset = heightToSet - cam.localPosition.y;
				camOffset.localPosition = new Vector3(camOffset.localPosition.x, camYOffset, camOffset.localPosition.z);
				camFollowerOffset.localPosition = new Vector3(camFollowerOffset.localPosition.x, camYOffset, camFollowerOffset.localPosition.z);
			}
		}
	}
}
