using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.YesNoSystem {
	public class HornTrigger : MonoBehaviour
	{
		[BoxGroup("Debug"), SerializeField] private bool isDebug = true;
		[BoxGroup("Debug"), ShowIf("isDebug", true), SerializeField] private Color triggerAreaColor = Color.blue;

		[BoxGroup("Variables"), SerializeField] private float triggerRadius = .25f;

		[BoxGroup("Components"), SerializeField] private DialogueInteractor interactor;
		[BoxGroup("Components"), SerializeField] private SphereCollider triggerArea;

		[BoxGroup("Audio"), SerializeField] private AudioClip hornSound;

		private AudioSource audioSource;

		private void Start()
		{
			audioSource = GetComponent<AudioSource>();
			audioSource.clip = hornSound;
			triggerArea.radius = triggerRadius;
		}

		private void OnTriggerEnter(Collider c)
		{
			if (c.GetComponent<Autohand.Hand>() != null)
			{
				interactor.Interact();

				if (audioSource.isPlaying == false)
					audioSource.Play();
			}
		}

		private void OnDrawGizmos()
		{
			if (isDebug && triggerArea != null)
			{
				Gizmos.color = triggerAreaColor;
				Gizmos.DrawSphere(triggerArea.transform.position, triggerRadius);
			}
		}
	}
}
