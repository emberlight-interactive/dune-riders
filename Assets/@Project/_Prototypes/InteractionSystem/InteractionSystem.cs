using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.InteractionSystem {
	public class InteractionInitiator : MonoBehaviour {
		InteractionTarget interactionTarget;
	}

	public abstract class InteractionTarget : MonoBehaviour {
		InteractionInitiator initiator;
		protected abstract void StartInteraction();
		protected abstract void EndInteraction();

		public void StartInteractionWith(InteractionInitiator initiator) {
			this.initiator = initiator;
			StartInteraction();
		}

		public void CloseInteraction() {
			initiator = null;
			EndInteraction();
		}
	}

	abstract class ResponseRequester<ResultType, TBehaviour> where TBehaviour : ResponseRequestMonoBehaviour<ResultType> {
		public delegate void HandleResult(ResultType result);
		public delegate void HandleCancel();
		public readonly HandleResult successCallback;
		public readonly HandleCancel cancelCallback;
		protected TBehaviour linkedBehaviour;

		ResultType result;

		public ResponseRequester(HandleResult successCallback, HandleCancel cancelCallback) {
			this.successCallback = successCallback;
			this.cancelCallback = cancelCallback;

			LinkBehaviour();
			RegisterBehaviourCallbacks();
		}

		public void Initiate() { linkedBehaviour.Initiate(); }
		public void ForceCancel() { linkedBehaviour.ForceCancel(); }

		void LinkBehaviour() {
			linkedBehaviour = MonoBehaviour.FindObjectOfType<TBehaviour>(true);
		}

		void RegisterBehaviourCallbacks() {
			linkedBehaviour.HandleResult = (ResultType result) => successCallback(result);
			linkedBehaviour.HandleCancel = () => cancelCallback();
		}
	}

	public abstract class ResponseRequestMonoBehaviour<ResultType> : MonoBehaviour {
		public delegate void HandleResultCallback(ResultType result);
		public delegate void HandleCancelCallback();
		public HandleResultCallback HandleResult { get; set; }
		public HandleCancelCallback HandleCancel { get; set; }

		public abstract void Initiate();
		public abstract void ForceCancel();
	}
}
