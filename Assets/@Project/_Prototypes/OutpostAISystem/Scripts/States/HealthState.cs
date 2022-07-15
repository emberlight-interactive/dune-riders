using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace DuneRiders.OutpostAI.State {
	[DisallowMultipleComponent]
	public class HealthState : MonoBehaviour
	{
		[ReadOnly] public int health = 100;
	}
}
