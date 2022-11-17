using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	[DefaultExecutionOrder(-100)]
	[RequireComponent(typeof(Camera))]
	public class LayerDistanceCullDefault : MonoBehaviour
	{
		Camera cameraPerformingCulling;
		[SerializeField] float distanceToUntilCull;

		void Awake() {
			cameraPerformingCulling = GetComponent<Camera>();

			var distances = cameraPerformingCulling.layerCullDistances;

			for (int i = 0; i < distances.Length; i++) {
				distances[i] = distanceToUntilCull;
			}

			cameraPerformingCulling.layerCullDistances = distances;
		}
	}
}
