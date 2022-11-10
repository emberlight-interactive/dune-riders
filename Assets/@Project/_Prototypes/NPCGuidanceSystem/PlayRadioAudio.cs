using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.NPCGuidanceSystem {
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(AudioHighPassFilter))]
	public class PlayRadioAudio : MonoBehaviour
	{
		[SerializeField] AudioClip radioBuzz;
		AudioClip clipToPlay;
		AudioSource audioSource;

		void Awake() {
			audioSource = GetComponent<AudioSource>();
		}

		public void PlayRadioedClip(AudioClip audioClip) {
			clipToPlay = audioClip;
			audioSource.clip = radioBuzz;
			audioSource.Play();
			Invoke(nameof(PlayClip), radioBuzz.length + 0.2f);
		}

		void PlayClip() {
			audioSource.clip = clipToPlay;
			audioSource.Play();
		}
	}
}
