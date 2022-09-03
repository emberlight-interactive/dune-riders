using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;

namespace DuneRiders.RiderAI {
	public class TargetInitializer : MonoBehaviour
	{
		void Awake() {
			var targetTransform = GetComponentInParent<TargetTransform>();
			if (!targetTransform) return;

			targetTransform.target = transform;
		}
	}
}
