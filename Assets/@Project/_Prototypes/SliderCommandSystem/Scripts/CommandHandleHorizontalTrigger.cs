using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Prototype
{
	public class CommandHandleHorizontalTrigger : MonoBehaviour
	{
		private BoxCollider triggerArea;

		private void Start()
		{
			triggerArea = GetComponent<BoxCollider>();
		}

		private void OnDrawGizmos()
		{   

		}
	}
}
