using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaia;

namespace DuneRiders.Combinations {
    public class EnableComponentsOnTerrainLoad : MonoBehaviour
    {
        TerrainScene terrainScene;
		FloatingPointFix floatingPointFix;
		Vector3 originalPosition;
        public List<MonoBehaviour> componentsToActivate = new List<MonoBehaviour>();

		void Awake() {
			var terrainManager = GaiaUtils.GetTerrainLoaderManagerObject(false);
			floatingPointFix = FindObjectOfType<FloatingPointFix>();

			if (terrainManager == null) WrapUpBulkComponentActivation();
		}

        void Start() {
			InitOriginalPosition();

            if (TerrainLoaderManager.TerrainScenes.Count > 0) {
                foreach (TerrainScene p in TerrainLoaderManager.TerrainScenes) {
                    if (p.m_bounds.Contains(originalPosition)){
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

		void InitOriginalPosition() {
			if (floatingPointFix == null) originalPosition = transform.position;
			else originalPosition = floatingPointFix.ConvertToOriginalSpace(transform.position);
		}
    }
}
