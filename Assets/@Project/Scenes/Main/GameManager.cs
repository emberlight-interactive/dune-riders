using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	public class GameManager : MonoBehaviour {
		private static GameManager instance;
		public static GameManager Instance { get { return instance; } }

		public string persistenceFileName = "SaveFile1.es3";
		public int gasBonusesUsed = 0;

		void Awake() {
			if (instance != null && instance != this) {
				Destroy(this.gameObject);
			} else {
				instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
		}
	}
}
