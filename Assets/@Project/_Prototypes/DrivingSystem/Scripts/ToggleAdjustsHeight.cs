using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DuneRiders.DrivingSystem {
	// todo: Can move head down to offset higher above the rider
	public class ToggleAdjustsHeight : MonoBehaviour
	{
		[SerializeField] Transform cam;
		[SerializeField] Transform camOffset;
		[SerializeField] Transform camFollowerOffset;
		[SerializeField] InputActionProperty leftToggleThumbstick;

		[SerializeField] float maxYOffset = 1.3f;
		[SerializeField] float minYOffset = 0.3f;
		[SerializeField] float heightChangeSpeed = 1f;

		void Start() {
			leftToggleThumbstick.action.Enable();
		}

		void Update() {
			if (ToggleDown() || ToggleUp()) {
				if (ToggleUp() && camOffset.localPosition.y + cam.localPosition.y >= maxYOffset) return;
				if (ToggleDown() && camOffset.localPosition.y + cam.localPosition.y <= minYOffset) return;

				var offsetToApply = Time.deltaTime * heightChangeSpeed;
				if (ToggleDown()) offsetToApply = -offsetToApply;

				camOffset.localPosition = new Vector3(camOffset.localPosition.x, camOffset.localPosition.y + offsetToApply, camOffset.localPosition.z);
				camFollowerOffset.localPosition = new Vector3(camFollowerOffset.localPosition.x, camFollowerOffset.localPosition.y + offsetToApply, camFollowerOffset.localPosition.z);
			}
		}

		bool ToggleUp() {
			return leftToggleThumbstick.action.ReadValue<Vector2>().y > 0.95f;
		}

		bool ToggleDown() {
			return leftToggleThumbstick.action.ReadValue<Vector2>().y < -0.95f;
		}
	}
}
