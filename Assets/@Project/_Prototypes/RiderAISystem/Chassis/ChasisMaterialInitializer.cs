using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI {
	[RequireComponent(typeof(MeshRenderer))]
	public class ChasisMaterialInitializer : MonoBehaviour
	{
		[SerializeField] Material banditsHullColor;

		void Awake() {
			if (GetComponentInParent<Rider>().allegiance == AI.Allegiance.Bandits) {
				GetComponent<MeshRenderer>().material = banditsHullColor;
			}
		}
	}
}
