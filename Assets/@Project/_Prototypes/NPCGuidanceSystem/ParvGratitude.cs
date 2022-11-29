using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.NPCGuidanceSystem {
	[RequireComponent(typeof(PlayRadioAudio))]
	public class ParvGratitude : MonoBehaviour
	{
		[SerializeField] List<AudioClip> gratitudes = new List<AudioClip>();
		[SerializeField] float gratitudeDelay = 5f;

		public void PlayRandomGratitude() {
			GetComponent<PlayRadioAudio>().PlayRadioedClip(gratitudes[Random.Range(0, gratitudes.Count)]);
		}

		public void PlayRandomGratitudeAfterDelay() {
			Invoke(nameof(PlayRandomGratitude), gratitudeDelay);
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
