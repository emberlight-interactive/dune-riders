using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

namespace DuneRiders.PauseSystem {
	public class PauseEvents : MonoBehaviour
	{
		[SerializeField] Hand leftHand;
		[SerializeField] Hand rightHand;

		[SerializeField] List<SkinnedMeshRenderer> skinnedMeshRenderersToToggle = new List<SkinnedMeshRenderer>();

		[SerializeField] Canvas pauseCanvas;

		bool shiftTrackingAwayFromPhysics = false;

		void Update() {
			if (shiftTrackingAwayFromPhysics) {
				rightHand.transform.position = rightHand.follow.position;
				rightHand.transform.rotation = rightHand.follow.rotation;
				leftHand.transform.position = leftHand.follow.position;
				leftHand.transform.rotation = leftHand.follow.rotation;
			}
		}

		public void Freeze() {
			Time.timeScale = 0.0f;
		}

		public void Unfreeze() {
			Time.timeScale = 1.0f;
		}

		public void ShiftTrackingAwayFromPhysics() {
			shiftTrackingAwayFromPhysics = true;
		}

		public void ShiftTrackingToPhysics() {
			shiftTrackingAwayFromPhysics = false;
		}

		public void ShowAllMeshes() {
			if (skinnedMeshRenderersToToggle.Count > 0) {
				foreach (var mesh in skinnedMeshRenderersToToggle) {
					mesh.enabled = true;
				}
			}
		}

		public void HideAllMeshes() {
			if (skinnedMeshRenderersToToggle.Count > 0) {
				foreach (var mesh in skinnedMeshRenderersToToggle) {
					mesh.enabled = false;
				}
			}
		}

		public void ShowPauseUI() {
			pauseCanvas.enabled = true;
		}

		public void HidePauseUI() {
			pauseCanvas.enabled = false;
		}
	}
}
