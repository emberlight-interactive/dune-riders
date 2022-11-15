using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DuneRiders {
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class ShowLossReason : MonoBehaviour
	{
		void Awake() {
			GetComponent<TextMeshProUGUI>().text = FindObjectOfType<GameManager>().gameLossReason;
		}
	}
}
