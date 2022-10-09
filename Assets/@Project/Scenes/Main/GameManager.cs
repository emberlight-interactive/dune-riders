using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders {
	public class GameManager : MonoBehaviour, IPersistent {
		[Serializable]
		class GameManagerSerializable {
			public int gasBonusesUsed;
		}

		private static GameManager instance;
		public static GameManager Instance { get { return instance; } }
		public bool DisablePersistence { get => false; }
		public string persistenceKey = "GameManager";

		public string persistenceFileName = "SaveFile1.es3";
		public int gasBonusesUsed = 0;
		public int maxAllowedGasBonuses = 3;
		public bool applyGasBonusOnMainSceneLoad = false;

		void Awake() {
			if (instance != null && instance != this) {
				Destroy(this.gameObject);
			} else {
				instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
		}

		public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save(persistenceKey, new GameManagerSerializable {
				gasBonusesUsed = this.gasBonusesUsed,
			});
		}

        public void Load(IPersistenceUtil persistUtil) {
			var loadedGameManager = persistUtil.Load<GameManagerSerializable>(persistenceKey);
			gasBonusesUsed = loadedGameManager.gasBonusesUsed;
		}
	}
}
