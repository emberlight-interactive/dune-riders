using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;

namespace DuneRiders.RiderAI {
	[RequireComponent(typeof(Turret))]
	public class GunnerInitializer : MonoBehaviour
	{
		void Awake() {
			var gunner = GetComponentInParent<Gunner>();
			if (!gunner) return;

			gunner.turret = GetComponent<Turret>();
		}
	}
}
