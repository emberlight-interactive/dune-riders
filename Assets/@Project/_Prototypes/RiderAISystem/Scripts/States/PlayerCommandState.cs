using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.Shared.PersistenceSystem;

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

		public class PlayerCommandGlobalState : MonoBehaviour, IPersistent
		{
			[Serializable]
			class PlayerCommandGlobalStateSerializable {
				public PlayerCommandState.Command command;
			}

			private static PlayerCommandGlobalState _instance;
			public static PlayerCommandGlobalState Instance { get { return _instance; } }
			public bool DisablePersistence { get => false; }
			string persistenceKey = "CurrentPlayerCommand";

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

			public void Save(IPersistenceUtil persistUtil) {
				persistUtil.Save<PlayerCommandGlobalStateSerializable>(persistenceKey, new PlayerCommandGlobalStateSerializable {
					command = this.command,
				});
			}

			// todo: does loading the halt command twice move it to new halt positions?
			public void Load(IPersistenceUtil persistUtil) {
				var loadedPlayerCommandGlobalStateSerializable = persistUtil.Load<PlayerCommandGlobalStateSerializable>(persistenceKey);
				command = loadedPlayerCommandGlobalStateSerializable.command;
			}
		}
	}
}
