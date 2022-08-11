using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

namespace DuneRiders.InteractionSystem.WaveInteraction {
	[RequireComponent(typeof(BoxCollider))]
	public class WaveResponseInterface : ResponseRequestMonoBehaviour<bool>
	{
		BoxCollider waveSpace;

		[SerializeField] Hand rightHand;

		bool active = false;
		public bool Active { get => active; }

		void Awake() {
			waveSpace = GetComponent<BoxCollider>();
			waveSpace.isTrigger = true;
		}

		public override void Initiate() { active = true; }
		public override void ForceCancel() { active = false; }

		void OnTriggerEnter(Collider c)
		{
			if (!Active) return;
			if (c.GetComponent<Hand>() == rightHand) {
				ForceCancel();
				HandleResult(true);
			}
		}

		void OnDrawGizmos() {
			Gizmos.color = new Vector4(1, 0, 1, 0.3f);
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}
	}

	class WaveResponseRequester : ResponseRequester<bool, WaveResponseInterface> {
		public WaveResponseRequester(HandleResult successCallback, HandleCancel cancelCallback) : base(successCallback, cancelCallback) {}

		public override void Initiate() { linkedBehaviour.Initiate(); }
		public override void ForceCancel() { linkedBehaviour.ForceCancel(); }
	}
}
