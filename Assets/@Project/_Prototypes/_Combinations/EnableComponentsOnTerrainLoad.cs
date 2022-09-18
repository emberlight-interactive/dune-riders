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
            if (terrainScene.m_regularLoadState == LoadState.Loaded) {
                WrapUpBulkComponentActivation();
            }
        }

		bool HaveTerrainsAlreadyBeenLoadedIn() { // todo: it seems this might be causing shit to fall through floors
			return (TerrainLoaderManager.TerrainScenes.Find(x => x.RegularReferences.Count > 0 && x.m_regularLoadState == LoadState.Loaded) != null);
		}
    }
}
