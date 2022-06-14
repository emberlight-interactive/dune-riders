using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRotation : MonoBehaviour
{
	[SerializeField] Transform sourceTranform;

	// Rotation boundries

    void LateUpdate()
    {
		transform.localRotation = Quaternion.Euler(
			0,
			sourceTranform.eulerAngles.y,
			0
		);
    }
}
