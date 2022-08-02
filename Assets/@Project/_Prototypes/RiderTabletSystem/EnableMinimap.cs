using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderTabletSystem {
	public class EnableMinimap : MonoBehaviour
	{
		[SerializeField] GameObject minimap;
		[SerializeField] GameObject minimapRig;
		[SerializeField] Transform minimapUI;
		[SerializeField] Canvas hostCanvas;

		public void Enable() {
			var eventCamera = hostCanvas.worldCamera;
			var minimapUIOriginalRotation = minimapUI.eulerAngles;
			minimapUI.eulerAngles = new Vector3(0, 0, 0);
			minimap.SetActive(true);
			minimapRig.SetActive(true);
			minimapUI.eulerAngles = minimapUIOriginalRotation;
			hostCanvas.worldCamera = eventCamera;

			EnableAllMapIcons();
		}

		void EnableAllMapIcons() {
			bl_MiniMapEntity[] mapIcons = GameObject.FindObjectsOfType<MapIcon>();
			for (int i = 0; i < mapIcons.Length; i++) {
				mapIcons[i].enabled = true;
			}
		}
	}
}
