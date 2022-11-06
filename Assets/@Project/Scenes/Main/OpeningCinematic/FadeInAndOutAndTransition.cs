using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DuneRiders {
	public class FadeInAndOutAndTransition : MonoBehaviour
	{
		[SerializeField] OVRScreenFade screenFade;
		[SerializeField] AudioSource fadeSource;
		[SerializeField] float fadeSourceMaxVolume;
		[SerializeField] UnityEvent transitionFinished = new UnityEvent();
		[SerializeField] float transitionLifeTime;

		void Start() {
			StartCoroutine(StartAudioFade(fadeSource, 2f, fadeSourceMaxVolume));
			Invoke(nameof(StartFadeOut), transitionLifeTime);
			Invoke(nameof(TransitionFinished), transitionLifeTime + 3f);
		}

		void StartFadeOut() {
			StartCoroutine(FadeOut(fadeSource, 3f));
			screenFade.FadeOut();
		}

		void TransitionFinished() {
			transitionFinished.Invoke();
		}

		public static IEnumerator StartAudioFade(AudioSource audioSource, float duration, float targetVolume)
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

		IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
			float startVolume = audioSource.volume;

			while (audioSource.volume > 0) {
				audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

				yield return null;
			}

			audioSource.Stop ();
			audioSource.volume = startVolume;
		}
	}
}
