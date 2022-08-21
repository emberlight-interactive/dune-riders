using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuneRiders.BanditSpawnerSystem {
	public class SpawnFormationPosition : MonoBehaviour
	{
		void OnDrawGizmos() {
			Gizmos.color = Color.magenta;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawSphere(Vector3.zero, 0.25f);
		}

		void OnEnable() {
			SetYAxisOfPositionToGroundLevel();
		}

		void SetYAxisOfPositionToGroundLevel() {
			RaycastHit hit;
			if (Physics.Raycast(new Vector3(transform.position.x, 1000, transform.position.z), -Vector3.up, out hit)) {
				transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
			}
		}
	}
}
