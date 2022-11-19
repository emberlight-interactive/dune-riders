using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DuneRiders.InteractionSystem.OptionSelectionInteraction {
	public class OptionButton : MonoBehaviour
	{
		[SerializeField] Button button;
		[SerializeField] TextMeshProUGUI buttonText;
		GameObject injectedGameObject;

		public void SetGameObject(GameObject injectableGameObject) {
			if (injectedGameObject != null) Destroy(injectedGameObject);
			if (injectableGameObject != null) injectedGameObject = Instantiate(injectableGameObject, button.transform);
		}

		public void SetText(string text) {
			buttonText.text = text;
		}

		public string GetText() {
			return buttonText.text;
		}

		public Button GetButton() {
			return button;
		}
	}
}
