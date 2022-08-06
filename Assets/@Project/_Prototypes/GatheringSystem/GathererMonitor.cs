using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DuneRiders.GatheringSystem {
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class GathererMonitor : MonoBehaviour
	{
		[SerializeField] Gatherer gatherer;
		TextMeshProUGUI tmpro;

		void Start()
		{
			tmpro = GetComponent<TextMeshProUGUI>();
		}

		void Update() {
			tmpro.text = "Precious Metal: " + gatherer.GetManager(Gatherer.SupportedResources.PreciousMetal).Amount();
		}
	}
}
