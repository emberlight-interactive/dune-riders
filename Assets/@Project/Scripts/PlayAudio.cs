using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	[RequireComponent(typeof(AudioSource))]
	public class PlayAudio : MonoBehaviour
	{
		public float delayBeforePlay = 0f;
		public float startTimestamp = 0f;

		void Awake() {
			GetComponent<AudioSource>().time = startTimestamp;
			if (delayBeforePlay > 0f) GetComponent<AudioSource>().PlayDelayed(delayBeforePlay);
		}
	}
}
