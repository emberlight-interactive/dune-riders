using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaia;

namespace DuneRiders.Combinations {
    public class EnableComponentsOnTerrainLoad : MonoBehaviour
    {
        TerrainScene terrainScene;
		TerrainLoader terrainLoader;
		FloatingPointFix floatingPointFix;
		Vector3 originalPosition;
        public List<MonoBehaviour> componentsToActivate = new List<MonoBehaviour>();

		void Awake() {
			var terrainManager = GaiaUtils.GetTerrainLoaderManagerObject(false);
			terrainLoader = FindObjectOfType<TerrainLoader>();
			floatingPointFix = FindObjectOfType<FloatingPointFix>();

			if (terrainManager == null || terrainLoader == null) WrapUpBulkComponentActivation();
		}

        void Start() {
            InitCurrentTerrainScene();
			transform.hasChanged = false;

			if (terrainScene == null) {
				WrapUpBulkComponentActivation();
			}
        }

		void InitCurrentTerrainScene() {
			if (TerrainLoaderManager.TerrainScenes.Count > 0) {
                foreach (TerrainScene p in TerrainLoaderManager.TerrainScenes) {
                    if (p.m_bounds.Contains(GetOriginalPosition())){
                        terrainScene = p;
						break;
                    }
                }
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
            } else if (transform.hasChanged) {
				InitCurrentTerrainScene();
				transform.hasChanged = false;
			}
        }

		Vector3 GetOriginalPosition() {
			if (floatingPointFix == null) return transform.position;
			else return floatingPointFix.ConvertToOriginalSpace(transform.position);
		}
    }
}
