using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuneRiders.OutpostAI {
	public class StructureHealthToHealthBar : MonoBehaviour
	{
		[SerializeField] StructureHealth structureHealth;
		[SerializeField] Slider healthBar;

		void Update() {
			healthBar.value = (float) structureHealth.health / structureHealth.MaxHealth;
		}
	}
}
