using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.Shared;
using UnityEngine.InputSystem;

namespace DuneRiders.YesNoSystem {
	public enum HandDecision
	{
		ThumbsUp,
		ThumbsDown,
		None
	}

	public class HandDecisionTrigger : MonoBehaviour // todo: Double tapping does not properly exit dialouge
	{
		[BoxGroup("Debug"), SerializeField] private bool isDebug = true;
		[BoxGroup("Debug"), ShowIf("isDebug", true), SerializeField] private HandDecision handDecision;
		[BoxGroup("Debug"), ShowIf("isDebug", true), SerializeField] private Color emptyTriggerColor = Color.blue;
		[BoxGroup("Debug"), ShowIf("isDebug", true), SerializeField] private Color occupiedTriggerColor = Color.yellow;

		[BoxGroup("Variables"), SerializeField] private float decisionRegisterAngle = 40;

		[BoxGroup("Components"), SerializeField] private BoxCollider triggerArea;

		[BoxGroup("Input Actions"), SerializeField] private InputActionProperty rightControllerTrigger;
		[BoxGroup("Input Actions"), SerializeField] private InputActionProperty rightControllerGrip;

		private bool handInside = false;
		private bool interactive = false;

		private void Start()
		{
			triggerArea = GetComponent<BoxCollider>();
			handDecision = HandDecision.None;
			rightControllerTrigger.action.Enable();
			rightControllerGrip.action.Enable();
		}

		public void SetInteractive(bool state)
		{
			interactive = state;
		}

		public HandDecision GetHandState()
		{
			return handDecision;
		}

		private void OnTriggerEnter(Collider c)
		{
			if (interactive == false)
				return;

			if (c.GetComponent<Autohand.Hand>() != null)
			{
				handInside = true;
			}
		}

		private void OnTriggerStay(Collider c)
		{
			if (interactive == false)
				return;

			if (c.GetComponent<Autohand.Hand>() != null && rightControllerGrip.action.IsPressed() && rightControllerTrigger.action.IsPressed())
			{
				Vector3 eulerAngles = c.gameObject.transform.rotation.eulerAngles;
				float result = eulerAngles.z - Mathf.CeilToInt(eulerAngles.z / -360f) * 360f;

				if (result > -decisionRegisterAngle / 2 && result < decisionRegisterAngle)
					handDecision = HandDecision.ThumbsUp;
				else if (result > 180 - decisionRegisterAngle / 2 && result < 180 + decisionRegisterAngle / 2)
					handDecision = HandDecision.ThumbsDown;
				else
					handDecision = HandDecision.None;
			}
		}

		private void OnTriggerExit(Collider c)
		{
			if (c.GetComponent<Autohand.Hand>() != null)
			{
				handInside = false;
				handDecision = HandDecision.None;
			}
		}

		private void OnDrawGizmos()
		{
			if (interactive == false)
				return;

			if (triggerArea != null)
			{
				Color c = handInside ? c = emptyTriggerColor : c = occupiedTriggerColor;
				Gizmos.color = c;
				Gizmos.DrawCube(transform.position, triggerArea.size);
			}
		}
	}
}
