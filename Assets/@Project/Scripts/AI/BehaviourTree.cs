using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.AI {
	public abstract class BehaviourTree : MonoBehaviour
	{
		[SerializeField, ReadOnly] protected List<Actioner> currentlyActiveActioners = new List<Actioner>();
		protected abstract void ProcessBehaviourTree();
		protected abstract (System.Type, string, System.Object)[] priorityStates {get;}
		List<System.Object> priorityStatesCache = new List<System.Object>();
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
			var actionerFromActiveList = currentlyActiveActioners.Find((thisActioner) => thisActioner == actioner);

			if (actionerFromActiveList == null) {
				currentlyActiveActioners.Add(actioner);
				actioner.StartAction();
			} else if (!actionerFromActiveList.currentlyActive) {
				actionerFromActiveList.StartAction();
			}

			if (currentlyActiveActioners.Count > 1) { // Clear all other actioners in list
				currentlyActiveActioners.RemoveAll((thisActioner) => {
					if (thisActioner != actioner) {
						thisActioner.EndAction();
						return true;
					}

					return false;
				});
			}
		}

		protected void SetActionersActive(Actioner[] actioners) {
			for (int i = 0; i < actioners.Length; i++) { // Add + activate / reactivate existing deactivated actioners
				var actionerFromActiveList = currentlyActiveActioners.Find((thisActioner) => thisActioner == actioners[i]);

				if (actionerFromActiveList == null) {
					currentlyActiveActioners.Add(actioners[i]);
					actioners[i].StartAction();
				} else if (!actionerFromActiveList.currentlyActive) {
					actionerFromActiveList.StartAction();
				}
			}

			if (currentlyActiveActioners.Count > 1) { // Clear all other actioners not included in our added actioners array
				currentlyActiveActioners.RemoveAll((thisActioner) => {
					for (int i = 0; i < actioners.Length; i++) {
						if (thisActioner == actioners[i]) return false;
					}

					thisActioner.EndAction();
					return true;
				});
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
