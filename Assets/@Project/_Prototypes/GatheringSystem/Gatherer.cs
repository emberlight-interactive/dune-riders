using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.GatheringSystem {
	[RequireComponent(typeof(Rigidbody))]
	public class Gatherer : MonoBehaviour
	{
		[SerializeField, ReadOnly] int preciousMetal = 0;
		[SerializeField] int preciousMetalLimit = 300;

		public bool AddPreciousMetal(int amount) {
			if (preciousMetal + amount > preciousMetalLimit) return false;
			else {
				preciousMetal += amount;
				return true;
			}
		}

		public bool GetPreciousMetal(int amount) {
			if (preciousMetal - amount < 0) return false;
			else {
				preciousMetal -= amount;
				return true;
			}
		}

		public int PreciousMetalAmount() { return preciousMetal; }
	}
}
