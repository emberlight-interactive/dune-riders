using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.Shared.PersistenceSystem;
using DuneRiders.PauseSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	[RequireComponent(typeof(OpenTutorialPage))]
	public class OpenTutorial : MonoBehaviour, IPersistent
	{
		[Serializable]
		class OpenTutorialSerializable {
			public bool opened;
		}

		OpenTutorialPage openTutorialPage;
		public bool DisablePersistence { get => false; }
		string persistenceKey = "StartingTutorial";
		bool opened = false;

		void Awake() {
			openTutorialPage = GetComponent<OpenTutorialPage>();
		}

		public void OpenStartingTutorial() {
			if (!opened) openTutorialPage.OpenDrivingTutorial();
			opened = true;
		}


		public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save(persistenceKey, new OpenTutorialSerializable {
				opened = this.opened,
			});
		}

        public void Load(IPersistenceUtil persistUtil) {
			var loadedOpenTutorial = persistUtil.Load<OpenTutorialSerializable>(persistenceKey);
			opened = loadedOpenTutorial.opened;
		}
	}
}
