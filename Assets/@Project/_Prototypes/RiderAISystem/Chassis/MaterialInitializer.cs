using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI {
	[RequireComponent(typeof(MeshRenderer))]
	public class MaterialInitializer : MonoBehaviour
	{
		[SerializeField] Material banditsHullColor;
		[SerializeField] bool firstMaterial = false;

		void Awake() {
			if (GetComponentInParent<Rider>().allegiance == AI.Allegiance.Bandits) {
				if (firstMaterial) {
					Material[] mats = GetComponent<MeshRenderer>().materials;
					mats[0] = banditsHullColor;
					GetComponent<MeshRenderer>().materials = mats;
				} else {
					GetComponent<MeshRenderer>().material = banditsHullColor;
				}
			}
		}
	}
}
