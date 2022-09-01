using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.POISystem {
	public class LootLocationGizmo : MonoBehaviour
	{
		void OnDrawGizmos() {
			Gizmos.color = new Vector4(1, 0, 1, 0.3f);
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
		}
	}
}
