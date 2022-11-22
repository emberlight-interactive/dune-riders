using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	[DefaultExecutionOrder(-500)]
	public class RunInBackground : MonoBehaviour
	{
		void Awake() {
			Application.runInBackground = true;
		}
	}
}
