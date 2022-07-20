using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePivot : MonoBehaviour
{
	private Quaternion startingRot;

	private void Start()
	{
		startingRot = transform.rotation;
	}
	void Update()
	{
		//Yes I know this is bad it's just for prototype if we decide the feature is worth while I will fix.=D
		transform.rotation = startingRot;
	}
}
