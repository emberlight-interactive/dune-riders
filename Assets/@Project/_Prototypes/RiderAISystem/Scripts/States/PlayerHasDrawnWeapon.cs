using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	public class PlayerHasDrawnWeapon : MonoBehaviour
	{
		PlayerHasDrawnWeaponGlobalState globalState;
		public bool isPlayerWeaponDrawn { get => globalState.isPlayerWeaponDrawn; }

		void Awake() {
			InitializeGlobalState();
		}

		void InitializeGlobalState() {
			PlayerHasDrawnWeaponGlobalState existingGlobalState = FindObjectOfType<PlayerHasDrawnWeaponGlobalState>();
			if (existingGlobalState != null) {
				globalState = existingGlobalState;
				return;
			}

			globalState = new GameObject("PlayerHasDrawnWeaponGlobalState").AddComponent<PlayerHasDrawnWeaponGlobalState>();
		}

		public class PlayerHasDrawnWeaponGlobalState : MonoBehaviour
		{
			private static PlayerHasDrawnWeaponGlobalState _instance;
			public static PlayerHasDrawnWeaponGlobalState Instance { get { return _instance; } }

			private void Awake()
			{
				if (_instance != null && _instance != this)
				{
					Destroy(this.gameObject);
				} else {
					_instance = this;
					FindObjectOfType<PlayerHasDrawnWeaponUpdater>()?.AttachMyself(this);
				}
			}

			public bool isPlayerWeaponDrawn = true;
		}
	}
}
