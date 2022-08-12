using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.InteractionSystem {
	[RequireComponent(typeof(SphereCollider))]
	[RequireComponent(typeof(Rigidbody))]
	public class InteractionInitiator : MonoBehaviour {
		InteractionTarget interactionTarget;
		SphereCollider interactorCollider;

		void Awake() {
			interactorCollider = GetComponent<SphereCollider>();
			interactorCollider.isTrigger = false;
			GetComponent<Rigidbody>().isKinematic = true;
			GetComponent<Rigidbody>().useGravity = false;
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().radius * transform.lossyScale.x);
		}

		private void OnTriggerEnter(Collider c)
		{
			if (c.GetComponent<InteractionTarget>() && interactionTarget == null)
			{
				interactionTarget = c.GetComponent<InteractionTarget>();
				interactionTarget.StartInteractionWith(this);
			}
		}

		private void OnTriggerExit(Collider c)
		{
			if (c.GetComponent<InteractionTarget>())
			{
				interactionTarget.CloseInteraction();
				interactionTarget = null;
			}
		}
	}
}
