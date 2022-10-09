using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuneRiders {
	public class ButtonClickerHelper : MonoBehaviour
	{
		public bool manuallyClickButton = false;

		void OnValidate() {
			if (manuallyClickButton) {
				manuallyClickButton = false;
				GetComponent<Button>().onClick.Invoke();
			}
		}
	}
}
