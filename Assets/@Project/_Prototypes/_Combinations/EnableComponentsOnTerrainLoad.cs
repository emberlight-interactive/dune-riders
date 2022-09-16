using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaia;

namespace DuneRiders.Combinations {
    public class EnableComponentsOnTerrainLoad : MonoBehaviour
    {
        TerrainScene terrainScene;
        public List<MonoBehaviour> componentsToActivate = new List<MonoBehaviour>();

		void Awake() {
			var sessionManager = GaiaSessionManager.GetSessionManager(false, false);
			if (sessionManager == null || HaveTerrainsAlreadyBeenLoadedIn()) {
				WrapUpBulkComponentActivation();
			}
		}

        void Start() {
            if (TerrainLoaderManager.TerrainScenes.Count > 0) {
                foreach (TerrainScene p in TerrainLoaderManager.TerrainScenes) {
                    if (p.m_bounds.Contains(transform.position)){
                        terrainScene = p;
						break;
                    }
                }
            }

			if (terrainScene == null) {
				WrapUpBulkComponentActivation();
			}
        }

		void ActivateAllComponents() {
			foreach (var component in componentsToActivate) {
				component.enabled = true;
			}
		}

		void WrapUpBulkComponentActivation() {
			ActivateAllComponents();
			Destroy(this);
		}

        void Update() {
			Debug.Log(gameObject.name + " " + terrainScene.m_regularLoadState);
			Debug.Log(gameObject.name + " " + terrainScene.GetTerrainName());
            if (terrainScene.m_regularLoadState == LoadState.Loaded) {
                WrapUpBulkComponentActivation();
            }
        }

		bool HaveTerrainsAlreadyBeenLoadedIn() {
			return (TerrainLoaderManager.TerrainScenes.Find(x => x.RegularReferences.Count > 0 && x.m_regularLoadState == LoadState.Loaded) != null);
		}
    }
}
