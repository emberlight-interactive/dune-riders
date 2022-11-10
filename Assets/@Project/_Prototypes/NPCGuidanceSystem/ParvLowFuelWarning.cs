using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.NPCGuidanceSystem {
	[RequireComponent(typeof(AudioSource))]
	public class ParvLowFuelWarning : MonoBehaviour
	{
		[SerializeField] List<AudioClip> warnings = new List<AudioClip>();
		[SerializeField] AudioClip radioBuzz;
		AudioSource audioSource;

		void Awake() {
			audioSource = GetComponent<AudioSource>();
		}

		public void PlayRandomWarning() {
			audioSource.clip = radioBuzz;
			audioSource.Play();
			Invoke(nameof(PlayParvatiVoice), radioBuzz.length + 0.2f);
		}

		void PlayParvatiVoice() {
			audioSource.clip = warnings[Random.Range(0, warnings.Count)];
			audioSource.Play();
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
