using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCameraLocalPosition : MonoBehaviour
{
	public Transform cameraToMatch;

    void LateUpdate()
    {
		transform.localPosition = cameraToMatch.localPosition;
		transform.localRotation = cameraToMatch.localRotation;
    }
}
