using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class EnableComponentsOnPlayerLoad : MonoBehaviour
	{
		PlayerLoaded playerLoaded;
        public List<MonoBehaviour> componentsToActivate = new List<MonoBehaviour>();

		void Awake() {
			playerLoaded = FindObjectOfType<PlayerLoaded>();

			if (playerLoaded == null) WrapUpBulkComponentActivation();
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
            if (playerLoaded.loaded) {
                WrapUpBulkComponentActivation();
            }
        }
	}
}
