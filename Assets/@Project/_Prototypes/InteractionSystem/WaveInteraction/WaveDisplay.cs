using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuneRiders.InteractionSystem.WaveInteraction {
	public class WaveDisplay : MonoBehaviour
	{
		[SerializeField] Image waveImage;
		[SerializeField] WaveResponseInterface waveResponseInterface;

		void Update() {
			if (waveResponseInterface.Active) {
				waveImage.enabled = true;
			} else {
				waveImage.enabled = false;
			}
		}
	}
}
