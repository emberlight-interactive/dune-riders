using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;

namespace DuneRiders.RiderAI.BehaviourTree {
	public abstract class BehaviourTree : MonoBehaviour
	{
		Actioner currentlyActiveActioner;
		protected abstract void ProcessBehaviourTree();
		protected abstract (System.Type, string, System.Object)[] priorityStates {get;}
		List<System.Object> priorityStatesCache = new List<System.Object>();

		void Start() {
			StartCoroutine(BehaviourTreeCoroutine());
		}

		void FixedUpdate() {
			if (HaveUpdatesOccuredForPriorityStates()) {
				ProcessBehaviourTree();
			}
		}

		IEnumerator BehaviourTreeCoroutine()
		{
			while (true) {
				ProcessBehaviourTree();
				yield return new WaitForSeconds(2.5f);
			}
		}

		protected void SetActionerActive(Actioner actioner) {
			if (currentlyActiveActioner == null) {
				currentlyActiveActioner = actioner;
				currentlyActiveActioner.StartAction();
			} else if (currentlyActiveActioner == actioner && !currentlyActiveActioner.currentlyActive) {
				currentlyActiveActioner.StartAction();
			} else if (currentlyActiveActioner != actioner) {
				if (currentlyActiveActioner.currentlyActive) currentlyActiveActioner.EndAction();

				currentlyActiveActioner = actioner;
				currentlyActiveActioner.StartAction();
			}
		}

		bool HaveUpdatesOccuredForPriorityStates() {
			// Populate cache with current values on first run //
			if (priorityStates.Length != priorityStatesCache.Count) {
				for (int i = 0; i < priorityStates.Length; i++) {
					priorityStatesCache.Add(priorityStates[i].Item1.GetField(priorityStates[i].Item2).GetValue(priorityStates[i].Item3));
				}

				return false;
			}

			// On subsequent runs check if updates have happened to priority states //
			for (int i = 0; i < priorityStates.Length; i++) {
				var currentValue = priorityStates[i].Item1.GetField(priorityStates[i].Item2).GetValue(priorityStates[i].Item3);
				var cachedValue = priorityStatesCache[i];

				if (currentValue != cachedValue) {
					priorityStatesCache[i] = currentValue;
					return true;
				}
			}

			return false;
		}
	}
}
