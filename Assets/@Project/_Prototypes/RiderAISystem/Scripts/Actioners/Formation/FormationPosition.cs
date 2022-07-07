using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuneRiders.RiderAI.Actioners {
	public class FormationPosition : MonoBehaviour
	{
		void OnDrawGizmos() {
			Gizmos.color = Color.magenta;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawSphere(Vector3.zero, 0.25f);
		}
	}
}
