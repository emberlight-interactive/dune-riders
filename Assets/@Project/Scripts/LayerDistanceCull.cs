using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	[RequireComponent(typeof(Camera))]
	public class LayerDistanceCull : MonoBehaviour
	{
		Camera cameraPerformingCulling;
		[SerializeField] int layerIndex;
		[SerializeField] float distanceToUntilCull;

		void Awake() {
			cameraPerformingCulling = GetComponent<Camera>();

			var distances = cameraPerformingCulling.layerCullDistances;
			distances[layerIndex] = distanceToUntilCull;
			cameraPerformingCulling.layerCullDistances = distances;
		}

		void OnDrawGizmos() {
			Gizmos.color = new Vector4(1, 0, 0, 0.3f);
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one * distanceToUntilCull * 2);
		}
	}
}
