using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.UI;

public class DialogueInteractor : MonoBehaviour
{
	[BoxGroup("Variables"), SerializeField] private float radius = 5;
	[BoxGroup("Variables"), SerializeField] private float decisionTimer = 1;

	[BoxGroup("Components"), SerializeField] private HandDecisionTrigger handTrigger;
	[BoxGroup("Components"), SerializeField] private Canvas decisionProgressCanvas;
	[BoxGroup("Components"), SerializeField] private Image decisionProgressImage;

	[BoxGroup("Debug"), SerializeField] private bool isDebug = true;
	[BoxGroup("Debug"), SerializeField, ShowIf("isDebug", true)] private Color centerPointColor = Color.magenta;
	[BoxGroup("Debug"), SerializeField, ShowIf("isDebug", true)] private Color interactionRadiusColor = Color.green;

	private SphereCollider interactionTrigger;
	private bool targetInRange = false;
	private DialogueTarget currentTarget = null;
	private bool waitingForInput = false;

	private void Start()
	{
		interactionTrigger = GetComponent<SphereCollider>();
		interactionTrigger.radius = radius;
	}

	public void Interact()
	{
		if (currentTarget != null)
		{
			if (isDebug)
				Debug.Log("Interact");

			currentTarget.OnInteracted();

			StartCoroutine(WaitForInput());
		}
		else
		{
			if (isDebug)
				Debug.Log("There is no target to interact with");
		}
	}

	private IEnumerator WaitForInput()
	{
		waitingForInput = true;
		handTrigger.SetInteractive(true);
		float thumbsUpTimer = 0;
		decisionProgressCanvas.enabled = true;
		decisionProgressImage.fillAmount = 0;
		float thumbsDownTimer = 0;
		while (waitingForInput)
		{
			if (handTrigger.GetHandState() == HandDecision.ThumbsUp)
			{
				if (thumbsDownTimer > 0)
				{
					thumbsDownTimer = 0;
					decisionProgressImage.fillAmount = 0;
				}
				thumbsUpTimer += Time.deltaTime;
				decisionProgressImage.fillAmount = thumbsUpTimer / decisionTimer;

				if (thumbsUpTimer >= decisionTimer)
				{
					if (isDebug)
						Debug.Log("Selected thumbs up.");

					waitingForInput = false;

					OnInputRecieved(true);
				}
			}

			if (handTrigger.GetHandState() == HandDecision.ThumbsDown)
			{
				if (thumbsUpTimer > 0)
				{
					decisionProgressImage.fillAmount = 0;
					thumbsUpTimer = 0;
				}

				thumbsDownTimer += Time.deltaTime;
				decisionProgressImage.fillAmount = thumbsDownTimer / decisionTimer;

				if (thumbsDownTimer >= decisionTimer)
				{
					if (isDebug)
						Debug.Log("Selected thumbs down.");

					waitingForInput = false;

					OnInputRecieved(false);
				}
			}

			yield return null;
		}

		handTrigger.SetInteractive(false);
		decisionProgressCanvas.enabled = false;
		decisionProgressImage.fillAmount = 0;
	}

	private void OnInputRecieved(bool thumbsUp)
	{
		currentTarget.OnInputRecieved();
		handTrigger.SetInteractive(false);

		if (isDebug)
		{
			if (thumbsUp)
				Debug.Log("The outcome was: YES");
			else
				Debug.Log("The outcome was: NO");
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<DialogueTarget>() != null)
		{
			targetInRange = true;
			currentTarget = c.GetComponent<DialogueTarget>();
			currentTarget.EnableTarget();
		}
	}

	private void OnTriggerExit(Collider c)
	{
		if (c.GetComponent<DialogueTarget>() != null)
		{
			waitingForInput = false;
			targetInRange = false;
			currentTarget.DisableTarget();
			currentTarget = null;
			handTrigger.SetInteractive(false);
		}
	}

	private void OnDrawGizmos()
	{
		if (isDebug)
		{
			Gizmos.color = centerPointColor;
			Gizmos.DrawSphere(transform.position, .1f);

			Color sphereColor = Color.red;
			if (targetInRange)
				sphereColor = interactionRadiusColor;

			Gizmos.color = sphereColor;
			Gizmos.DrawWireSphere(transform.position, radius);
			Gizmos.color = new Color(sphereColor.r, sphereColor.g, sphereColor.b, .25f);
			Gizmos.DrawSphere(transform.position, radius);
		}
	}
}