using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Autohand;
using Sirenix.OdinInspector;

namespace DuneRiders.InteractionSystem.ThumbsUpDownInteraction {
	[RequireComponent(typeof(BoxCollider))]
	public class ThumbsUpDownResponseInterface : ResponseRequestMonoBehaviour<bool>
	{
		BoxCollider thumbsUpSpace;

		[SerializeField] Hand rightHand;
		[SerializeField] InputActionProperty rightControllerTrigger;
		[SerializeField] InputActionProperty rightControllerGrip;
		[SerializeField] InputActionProperty rightControllerPrimaryButton;
		[SerializeField] float secondsToConfirmChoice = 1f;
		[SerializeField, ReadOnly] float confirmationProgress = 0f;
		public float ConfirmationProgress { get => confirmationProgress; }
		Coroutine resultsLoader;

		enum ThumbPosition {
			Up,
			Down,
			Sideways
		}

		bool active = false;
		public bool Active { get => active; }

		void Awake() {
			thumbsUpSpace = GetComponent<BoxCollider>();
			thumbsUpSpace.isTrigger = true;
		}

		public override void Initiate() { active = true; }
		public override void ForceCancel() { ShutDownInterface(); }

		void ShutDownInterface(bool triggerCancelCallback = false) {
			active = false;
			ClearResultsLoaderRoutine();
			ResetConfirmationProgress();
			if (triggerCancelCallback) HandleCancel();
		}

		void ResetConfirmationProgress() { confirmationProgress = 0f; }

		void OnTriggerEnter(Collider c)
		{
			if (!Active) return;
			if (c.GetComponent<Hand>() == rightHand) resultsLoader = StartCoroutine(ResultsLoader());
		}

		void OnTriggerExit(Collider c)
		{
			if (!Active) return;
			if (c.GetComponent<Hand>() == rightHand) {
				ClearResultsLoaderRoutine();
				ResetConfirmationProgress();
			}
		}

		void ClearResultsLoaderRoutine() {
			StopCoroutine(resultsLoader);
			resultsLoader = null;
		}

		IEnumerator ResultsLoader() {
			ThumbPosition currentPosition = ThumbPosition.Sideways;

			while (confirmationProgress < 1f) {
				if (IsHandInThumbsUpShape()) {
					if (currentPosition != ThumbPosition.Sideways && currentPosition == GetHandOrientation()) {
						IncrementConfirmationProgress();
					} else {
						currentPosition = GetHandOrientation();
						ResetConfirmationProgress();
					}
				} else {
					ResetConfirmationProgress();
				}

				yield return null;
			}

			HandleResult(currentPosition == ThumbPosition.Up ? true : false);
			ResetConfirmationProgress();
			active = false;
		}

		ThumbPosition GetHandOrientation() {
			// 0 Is perfect thumbs up rotation // 180 is perfect down
			float zRotationIn360Degrees = rightHand.transform.rotation.eulerAngles.z;

			if (zRotationIn360Degrees > 300 || zRotationIn360Degrees < 60)
				return ThumbPosition.Up;
			else if (zRotationIn360Degrees > 120 && zRotationIn360Degrees < 240)
				return ThumbPosition.Down;
			else
				return ThumbPosition.Sideways;
		}

		bool IsHandInThumbsUpShape() {
			return rightControllerGrip.action.IsPressed() &&
				rightControllerTrigger.action.IsPressed() &&
				!rightControllerPrimaryButton.action.IsPressed();
		}

		void IncrementConfirmationProgress() {
			var incrementAmount = (1f / secondsToConfirmChoice) * Time.deltaTime;

			if (incrementAmount + confirmationProgress > 1f) confirmationProgress = 1f;
			else confirmationProgress += incrementAmount;
		}

		void OnDrawGizmos() {
			Gizmos.color = new Vector4(1, 0, 1, 0.3f);
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}
	}

	class ThumbsUpDownResponseRequester : ResponseRequester<bool, ThumbsUpDownResponseInterface> {
		public ThumbsUpDownResponseRequester(HandleResult successCallback, HandleCancel cancelCallback) : base(successCallback, cancelCallback) {}

		public override void Initiate() { linkedBehaviour.Initiate(); }
		public override void ForceCancel() { linkedBehaviour.ForceCancel(); }
	}
}
