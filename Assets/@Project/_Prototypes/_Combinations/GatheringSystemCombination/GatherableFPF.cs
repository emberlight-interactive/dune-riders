using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GatheringSystem;
using Gaia;

namespace DuneRiders.GatheringSystemCombination {
	[RequireComponent(typeof(ApplyFloatingPointFixWhenInContext))]
	public class GatherableFPF : Gatherable
	{
		FloatingPointFix floatingPointFix;

		void Awake() {
			if (GetComponent<FloatingPointFixMember>() != null) floatingPointFix = FindObjectOfType<FloatingPointFix>();
		}

		Vector3 _restingDestination;
		protected override Vector3 restingDestination { get => _restingDestination + floatingPointFix.totalOffset; set => _restingDestination = value - floatingPointFix.totalOffset; }
	}
}
