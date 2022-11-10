using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.NPCGuidanceSystem {
	[RequireComponent(typeof(PlayRadioAudio))]
	public class ParvGratitude : MonoBehaviour
	{
		[SerializeField] List<AudioClip> gratitudes = new List<AudioClip>();

		public void PlayRandomGratitude() {
			GetComponent<PlayRadioAudio>().PlayRadioedClip(gratitudes[Random.Range(0, gratitudes.Count)]);
		}

		public bool playGratitude = false;

		void OnValidate() {
			if (playGratitude) {
				playGratitude = false;
				PlayRandomGratitude();
			}
		}
	}
}
