using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaia;

namespace DuneRiders {
	[DefaultExecutionOrder(-300)]
	public class ApplyFloatingPointFixWhenInContext : MonoBehaviour
	{
		void Awake() {
			var terrainManager = GaiaUtils.GetTerrainLoaderManagerObject(false);
			if (terrainManager != null) {
				gameObject.AddComponent<FloatingPointFixMember>();
			}
		}
	}
}
