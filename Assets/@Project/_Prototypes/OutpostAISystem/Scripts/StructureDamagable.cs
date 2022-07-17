using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.OutpostAI {
	public class StructureDamagable : MonoBehaviour
	{
		[SerializeField] StructureHealth structureHealth;

		void OnCollisionEnter(Collision collision)
		{
			structureHealth.health -= 1;
		}
	}
}
