using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DuneRiders.InteractionSystem.RangeSelectionInteraction {
	public class RangeSelectionResponseInterface : ResponseRequestMonoBehaviour<int>
	{
		[SerializeField] Canvas hostCanvas;
		[SerializeField] Canvas selectionCanvas;
		[SerializeField] Slider slider;
		[SerializeField] TextMeshProUGUI maxValueText;
		[SerializeField] TextMeshProUGUI currentSelectionText;
		[SerializeField] Canvas confirmationCanvas;
		[SerializeField] TextMeshProUGUI confirmationText;

		int maxSelectionValue;
		public int MaxSelectionValue {
			get => maxSelectionValue;
			set => maxSelectionValue = value;
		}

		void Awake() {
			EnableAllGameObjects();
			HideAllCanvases();
		}

		public override void Initiate() {
			hostCanvas.enabled = true;
			selectionCanvas.enabled = true;
			slider.value = 0.5f;
			maxValueText.text = maxSelectionValue.ToString();
			StartCoroutine(UpdatedCurrentSelectionText());
		}

		public override void ForceCancel() {
			StopAllCoroutines();
			HideAllCanvases();
		}

		public void ConfirmValueSelection() {
			confirmationCanvas.enabled = true;
			confirmationText.text = $"Confirm Selection of {GetCurrentlySelectedValue()}";
		}

		public void YesToConfirmation() {
			var result = GetCurrentlySelectedValue();
			ForceCancel();
			HandleResult(result);
		}

		public void CancelValueSelection() {
			ForceCancel();
			HandleCancel();
		}

		public void NoToConfirmation() {
			confirmationCanvas.enabled = false;
		}

		IEnumerator UpdatedCurrentSelectionText() {
			while (true) {
				currentSelectionText.text = GetCurrentlySelectedValue().ToString();
				yield return null;
			}
		}

		int GetCurrentlySelectedValue() {
			var sliderValue = maxSelectionValue * slider.value;
			return sliderValue < 1 ? 1 : (int) sliderValue;
		}

		void HideAllCanvases() {
			hostCanvas.enabled = false;
			selectionCanvas.enabled = false;
			confirmationCanvas.enabled = false;
		}

		void EnableAllGameObjects() {
			selectionCanvas.gameObject.SetActive(true);
			confirmationCanvas.gameObject.SetActive(true);
		}
	}

	class RangeSelectionResponseRequester : ResponseRequester<int, RangeSelectionResponseInterface> {
		int maxValue;

		public RangeSelectionResponseRequester(HandleResult successCallback, HandleCancel cancelCallback, int maxValue) : base(successCallback, cancelCallback) {
			this.maxValue = maxValue;
		}

		public override void Initiate() {
			linkedBehaviour.MaxSelectionValue = maxValue;
			linkedBehaviour.Initiate();
		}

		public override void ForceCancel() { linkedBehaviour.ForceCancel(); }
	}

}
