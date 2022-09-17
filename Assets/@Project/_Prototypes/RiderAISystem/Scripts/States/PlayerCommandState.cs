using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	public class PlayerCommandState : MonoBehaviour
	{
		public enum Command {Charge, Follow, Halt};
		PlayerCommandGlobalState globalState;
		[ReadOnly] public Command command;

		void Awake() {
			InitializeGlobalState();
		}

		void Update() {
			command = globalState.command; // todo: Convert the global class field into a reference type enum??
		}

		void InitializeGlobalState() {
			PlayerCommandGlobalState existingGlobalState = FindObjectOfType<PlayerCommandGlobalState>();
			if (existingGlobalState != null) {
				globalState = existingGlobalState;
				return;
			}

			globalState = new GameObject("PlayerCommandGlobalState").AddComponent<PlayerCommandGlobalState>();
		}

		public class PlayerCommandGlobalState : MonoBehaviour
		{
			private static PlayerCommandGlobalState _instance;
			public static PlayerCommandGlobalState Instance { get { return _instance; } }

			private void Awake()
			{
				if (_instance != null && _instance != this)
				{
					Destroy(this.gameObject);
				} else {
					_instance = this;
				}
			}

			[SerializeField] public PlayerCommandState.Command command = Command.Follow;
		}
	}
}
