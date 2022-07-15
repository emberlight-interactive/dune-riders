using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.OutpostAI {
	public class StructureHealth : MonoBehaviour
	{
		public int health = 50;

		void FixedUpdate() {
			// todo: Do the turrets still register after destruction ??
			// Might need to create a "structure" state that monitors if it's host structure is deactivated
			// and add that to the behaviour tree
			if (health <= 0) gameObject.SetActive(false);
		}
	}
}
