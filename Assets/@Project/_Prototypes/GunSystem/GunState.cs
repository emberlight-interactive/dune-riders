using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.GunSystem {
	public class GunState : MonoBehaviour
	{
		public class AvailableActions {
			public bool canFire;
			public bool canAim;
			public bool canUnPack;
			public bool canPack;
			public bool canDeactivate;
			public bool canActivate;
		}

		public enum State {
			Firing,
			Ready,
			Reloading,
			Transitioning,
			Packed
		};

		Dictionary<State, AvailableActions> availableActionsPerState = new Dictionary<State, AvailableActions> {
			{
				State.Firing,
				new AvailableActions() {
					canFire = false,
					canAim = true,
					canUnPack = false,
					canPack = false,
					canDeactivate = false,
					canActivate = false,
				}
			},
			{
				State.Ready,
				new AvailableActions() {
					canFire = true,
					canAim = true,
					canUnPack = false,
					canPack = true,
					canDeactivate = false,
					canActivate = false,
				}
			},
			{
				State.Reloading,
				new AvailableActions() {
					canFire = false,
					canAim = true,
					canUnPack = false,
					canPack = true,
					canDeactivate = false,
					canActivate = false,
				}
			},
			{
				State.Transitioning,
				new AvailableActions() {
					canFire = false,
					canAim = false,
					canUnPack = false,
					canPack = false,
					canDeactivate = false,
					canActivate = false,
				}
			},
			{
				State.Packed,
				new AvailableActions() {
					canFire = false,
					canAim = false,
					canUnPack = true,
					canPack = false,
					canDeactivate = true,
					canActivate = true,
				}
			}
		};

		[ReadOnly] public State state = State.Packed;
		[ReadOnly] public AvailableActions availableActions { get => availableActionsPerState[state]; }
	}
}
