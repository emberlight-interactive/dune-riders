using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DuneRiders.RiderTabletSystem {
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class ShowOnSliderValue : MonoBehaviour
	{
		TextMeshProUGUI text;
		string textShowing;
		[SerializeField] Slider slider;
		[SerializeField] float valueAtOrAboveToShow;

		void Start() {
			text = GetComponent<TextMeshProUGUI>();
			textShowing = text.text;
		}

		void Update() {
			if (slider.value >= valueAtOrAboveToShow) {
				text.text = textShowing;
			} else {
				text.text = "";
			}
		}
	}
}
