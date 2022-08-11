using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuneRiders.InteractionSystem.ThumbsUpDownInteraction {
	public class ThumbsUpDownDisplay : MonoBehaviour
	{
		[SerializeField] private Canvas decisionProgressCanvas;
		[SerializeField] private Image decisionProgressImage;
		[SerializeField] private ThumbsUpDownResponseInterface thumbsUpDownResponseInterface;

		void Update() {
			if (thumbsUpDownResponseInterface.Active) {
				decisionProgressCanvas.enabled = true;
				decisionProgressImage.fillAmount = thumbsUpDownResponseInterface.ConfirmationProgress;
			} else {
				decisionProgressCanvas.enabled = false;
			}
		}
	}
}
