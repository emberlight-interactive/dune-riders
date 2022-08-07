using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DuneRiders.DrivingSystem {
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class SpeedDisplayUpdater : MonoBehaviour
	{
		[SerializeField] Rigidbody rb;
		TextMeshProUGUI textMeshPro;

		void Start()
		{
			textMeshPro = GetComponent<TextMeshProUGUI>();
		}

		void Update()
		{
			var kmH = Mathf.Abs(3.6f * rb.velocity.magnitude).ToString("0");
			textMeshPro.text = $"Speed\n{kmH} km/h";
		}
	}
}
