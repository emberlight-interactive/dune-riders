using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.NPCGuidanceSystem {
	[RequireComponent(typeof(PlayRadioAudio))]
	public class ParvLowFuelWarning : MonoBehaviour
	{
		[SerializeField] List<AudioClip> warnings = new List<AudioClip>();

		public void PlayRandomWarning() {
			GetComponent<PlayRadioAudio>().PlayRadioedClip(warnings[Random.Range(0, warnings.Count)]);
		}

		public bool playWarning = false;

		void OnValidate() {
			if (playWarning) {
				playWarning = false;
				PlayRandomWarning();
			}
		}
	}
}
