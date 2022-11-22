using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderTabletSystem {
	public class MapIcon : bl_MiniMapEntity
	{
		[SerializeField] bool hideIconWhenGameObjectIsDisabled = true;
		[SerializeField] float maxDistanceDetectable = 2000f;
		bl_MiniMap minimap;

		void Awake() {
			minimap = GameObject.FindObjectOfType<bl_MiniMap>(true);
			if (!minimap.gameObject.activeSelf) enabled = false;
		}

		void OnEnable() {
			if (hideIconWhenGameObjectIsDisabled) SetIconActiveIdempotent(true);
			StartCoroutine(HandleMapIconOutOfBounds());
		}

		void OnDisable() {
			StopAllCoroutines();
			if (hideIconWhenGameObjectIsDisabled) SetIconActiveIdempotent(false);
		}

		IEnumerator HandleMapIconOutOfBounds() {
			while (true) {
				yield return new WaitForSeconds(2f);

				if (IconOutOfBounds()) {
					SetIconActiveIdempotent(false);
				} else {
					SetIconActiveIdempotent(true);
				}
			}
		}

		void SetIconActiveIdempotent(bool setActive) {
			if (!setActive || (setActive && !IconOutOfBounds())) {
				SetActiveIcon(setActive);
			}
		}

		bool IconOutOfBounds() {
			return Vector3.Distance(minimap.transform.position, transform.position) > maxDistanceDetectable;
		}
	}
}
