using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	public class AudioSourceUtils
	{
		public IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
		{
			float currentTime = 0;
			float start = audioSource.volume;
			while (currentTime < duration)
			{
				currentTime += Time.deltaTime;
				audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
				yield return null;
			}
			yield break;
		}

		public IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
			float startVolume = audioSource.volume;

			while (audioSource.volume > 0) {
				audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

				yield return null;
			}

			audioSource.Stop();
			audioSource.volume = 0;
		}
	}
}
