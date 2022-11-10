using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.NPCGuidanceSystem {
	[RequireComponent(typeof(PlayRadioAudio))]
	public class ParvTutorialTips : MonoBehaviour, IPersistent // todo: Refactor parv so that she doesn't talk over herself on simultaneous events
	{
		[Serializable]
		class ParvTutorialTipsSerializable {
			public bool gatheringHuntingTipPlayed;
			public bool mercenaryTipPlayed;
			public bool outpostTipPlayed;
			public bool migrationTipPlayed;
		}

		[Serializable]
		class TutorialTip {
			public AudioClip audioClip;
			public bool played;
		}

		[SerializeField] TutorialTip gatheringHuntingTip;
		[SerializeField] TutorialTip mercenaryTip;
		[SerializeField] TutorialTip outpostTip;
		[SerializeField] TutorialTip migrationTip;

		PlayRadioAudio playRadioAudio;

		public bool DisablePersistence { get => false; }
		string persistenceKey = "ParvTutorialTips";

		void Awake() {
			playRadioAudio = GetComponent<PlayRadioAudio>();
		}

		public void PlayGatheringHuntingTip() { PlayTip(gatheringHuntingTip); }
		public void PlayMercenaryTip() { PlayTip(mercenaryTip); }
		public void PlayOutpostTip() { PlayTip(outpostTip); }
		public void PlayMigrationTip() { PlayTip(migrationTip); }

		void PlayTip(TutorialTip tutorialTip) {
			if (!tutorialTip.played) {
				if (playRadioAudio.usedAudioSource.isPlaying) {
					StartCoroutine(TryPlayingTipAgainLater(tutorialTip, (playRadioAudio.currentClipToPlay.length) + 2f));
					return;
				}

				tutorialTip.played = true;
				playRadioAudio.PlayRadioedClip(tutorialTip.audioClip);
			}
		}

		IEnumerator TryPlayingTipAgainLater(TutorialTip tutorialTip, float delayTime) {
			yield return new WaitForSeconds(delayTime);
			PlayTip(tutorialTip);
		}

		public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save(persistenceKey, new ParvTutorialTipsSerializable {
				gatheringHuntingTipPlayed = gatheringHuntingTip.played,
				mercenaryTipPlayed = mercenaryTip.played,
				outpostTipPlayed = outpostTip.played,
				migrationTipPlayed = migrationTip.played,
			});
		}

        public void Load(IPersistenceUtil persistUtil) {
			var loadedParvTutorialTips = persistUtil.Load<ParvTutorialTipsSerializable>(persistenceKey);
			gatheringHuntingTip.played = loadedParvTutorialTips.gatheringHuntingTipPlayed;
			mercenaryTip.played = loadedParvTutorialTips.mercenaryTipPlayed;
			outpostTip.played = loadedParvTutorialTips.outpostTipPlayed;
			migrationTip.played = loadedParvTutorialTips.migrationTipPlayed;
		}
	}
}
