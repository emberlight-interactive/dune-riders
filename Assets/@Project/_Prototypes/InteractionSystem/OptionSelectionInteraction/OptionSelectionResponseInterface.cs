using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DuneRiders.InteractionSystem.OptionSelectionInteraction {
	public class OptionSelectionResponseInterface : ResponseRequestMonoBehaviour<string>
	{
		[SerializeField] List<OptionButton> optionButtons = new List<OptionButton>();
		[SerializeField] Canvas hostCanvas;

		string[] options;
		public string[] Options {
			get => options;
			set => options = value;
		}

		void Awake() {
			ForceCancel();
		}

		public override void Initiate() {
			InitializeButtons(options);
			hostCanvas.enabled = true;
		}

		public override void ForceCancel() {
			hostCanvas.enabled = false;
			ClearButtons();
		}

		void InitializeButtons(string[] optionStrings) {
			for (int i = 0; i < optionStrings.Length; i++) {
				if (i >= optionButtons.Count) break;

				optionButtons[i].GetButton().gameObject.SetActive(true);
				optionButtons[i].GetButton().enabled = true;
				optionButtons[i].SetText(optionStrings[i]);

				var iSpentAnHourTryingToRealizeForLoopIntegersAreSomehowPassedByReferenceWithoutDoingThis = i;
				optionButtons[i].GetButton().onClick.RemoveAllListeners();
				optionButtons[i].GetButton().onClick.AddListener(() => HandleButtonPressed(optionStrings[iSpentAnHourTryingToRealizeForLoopIntegersAreSomehowPassedByReferenceWithoutDoingThis]));
			}
		}

		void ClearButtons() {
			foreach(var option in optionButtons) {
				option.GetButton().gameObject.SetActive(false);
				option.GetButton().enabled = false;
			}
		}

		void HandleButtonPressed(string optionText) {
			ForceCancel();
			HandleResult(optionText);
		}
	}

	class OptionSelectionResponseRequester : ResponseRequester<string, OptionSelectionResponseInterface> {
		string[] options;

		public OptionSelectionResponseRequester(HandleResult successCallback, HandleCancel cancelCallback, string[] options) : base(successCallback, cancelCallback) {
			this.options = options;
		}

		public override void Initiate() {
			linkedBehaviour.Options = options;
			linkedBehaviour.Initiate();
		}

		public override void ForceCancel() { linkedBehaviour.ForceCancel(); }
	}
}
