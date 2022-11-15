using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.PlayerRiderControllerCombination {
	[RequireComponent(typeof(AudioSource))]
	public class BattleAmbience : MonoBehaviour
	{
		[SerializeField] AudioClip battleLoop;
		AudioSource battleAudioSource;
		AudioSourceUtils audioSourceUtils;

		[SerializeField] float targetVolume = 0.7f;
		[SerializeField] float fadeDuration = 5f;


		void Awake() {
			audioSourceUtils = new AudioSourceUtils();
			battleAudioSource = GetComponent<AudioSource>();
			battleAudioSource.clip = battleLoop;
		}

		public void FadeInBattleMusic() {
			StopAllCoroutines();
			if (!battleAudioSource.isPlaying) battleAudioSource.Play();
			StartCoroutine(audioSourceUtils.FadeIn(battleAudioSource, fadeDuration * ((targetVolume - battleAudioSource.volume) / targetVolume), targetVolume));
		}

		public void FadeOutBattleMusic() {
			StopAllCoroutines();
			StartCoroutine(audioSourceUtils.FadeOut(battleAudioSource, fadeDuration * (battleAudioSource.volume / targetVolume)));
		}
	}
}
