using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchLocalPosition : MonoBehaviour
{
	public Transform transformToMatch;

    void LateUpdate()
    {
		transform.localPosition = transformToMatch.localPosition;
		transform.localRotation = transformToMatch.localRotation;
    }
}
