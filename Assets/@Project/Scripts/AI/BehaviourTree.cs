using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.AI {
	public abstract class BehaviourTree : MonoBehaviour
	{
		List<Actioner> resuableActionersList = new List<Actioner>();
		[SerializeField, ReadOnly] protected List<Actioner> currentlyActiveActioners = new List<Actioner>();
		protected abstract void ProcessBehaviourTree();
		protected abstract PriorityStateMonitor[] priorityStateMonitors { get; }
		bool hasBeenDisabled = false;

		void Start() {
			StartCoroutine(BehaviourTreeCoroutine());
		}

		void OnEnable() {
			if (hasBeenDisabled) StartCoroutine(BehaviourTreeCoroutine());
		}

		void OnDisable() {
			StopAllCoroutines();
			hasBeenDisabled = true;
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

		protected void SetActionersActive(Actioner actioner) {
			if (currentlyActiveActioners.Count >= 1) { // Clear all other actioners in list
				currentlyActiveActioners.RemoveAll((thisActioner) => {
					if (thisActioner != actioner) {
						thisActioner.EndAction();
						return true;
					}

					return false;
				});
			}

			var actionerFromActiveList = currentlyActiveActioners.Find((thisActioner) => thisActioner == actioner);

			if (actionerFromActiveList == null) {
				currentlyActiveActioners.Add(actioner);
				actioner.StartAction();
			} else if (!actionerFromActiveList.currentlyActive) {
				actionerFromActiveList.StartAction();
			}
		}

		/// <summary>Please reuse lists to help with reducing GCAlloc issues</summary>
		protected void SetActionersActive(List<Actioner> actioners) {
			if (currentlyActiveActioners.Count >= 1) { // Clear all other actioners not included in our added actioners array
				currentlyActiveActioners.RemoveAll((thisActioner) => {
					for (int i = 0; i < actioners.Count; i++) {
						if (thisActioner == actioners[i]) return false;
					}

					thisActioner.EndAction();
					return true;
				});
			}

			for (int i = 0; i < actioners.Count; i++) { // Add + activate / reactivate existing deactivated actioners
				var actionerFromActiveList = currentlyActiveActioners.Find((thisActioner) => thisActioner == actioners[i]);

				if (actionerFromActiveList == null) {
					currentlyActiveActioners.Add(actioners[i]);
					actioners[i].StartAction();
				} else if (!actionerFromActiveList.currentlyActive) {
					actionerFromActiveList.StartAction();
				}
			}
		}

		bool HaveUpdatesOccuredForPriorityStates() {
			foreach (var stateMonitor in priorityStateMonitors) {
				if (stateMonitor.StateChanged()) return true;
			}

			return false;
		}

		/// <summary>Helps with reducing GCAlloc issues</summary>
		protected List<Actioner> GenerateActionerList(Actioner actionerOne, Actioner actionerTwo) {
			resuableActionersList.Clear();
			resuableActionersList.Add(actionerOne);
			resuableActionersList.Add(actionerTwo);
			return resuableActionersList;
		}

		/// <summary>Helps with reducing GCAlloc issues</summary>
		protected List<Actioner> GenerateActionerList(Actioner actionerOne, Actioner actionerTwo, Actioner actionerThree) {
			resuableActionersList.Clear();
			resuableActionersList.Add(actionerOne);
			resuableActionersList.Add(actionerTwo);
			resuableActionersList.Add(actionerThree);
			return resuableActionersList;
		}
	}

	public abstract class PriorityStateMonitor {
		public abstract bool StateChanged();
	}
}
