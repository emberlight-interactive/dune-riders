using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderTabletSystem {
	public class MapIcon : bl_MiniMapEntity
	{
		[SerializeField] bool hideIconWhenGameObjectIsDisabled = true;

		void Awake() {
			var minimap = GameObject.FindObjectOfType<bl_MiniMap>(true);
			if (!minimap.gameObject.activeSelf) enabled = false;
		}

		void OnEnable() {
			if (hideIconWhenGameObjectIsDisabled) SetActiveIcon(true);
		}

		void OnDisable() {
			if (hideIconWhenGameObjectIsDisabled) SetActiveIcon(false);
		}
	}
}
