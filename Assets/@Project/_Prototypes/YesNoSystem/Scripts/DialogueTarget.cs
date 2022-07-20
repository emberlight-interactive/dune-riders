using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace DuneRiders.YesNoSystem {
	public abstract class DialogueTarget : MonoBehaviour
	{
		[BoxGroup("Variables"), TextArea, SerializeField] private string textContent;
		[BoxGroup("Components"), SerializeField] private DialogueText dialogueText;

		private bool active;

		public void OnInteracted()
		{
			if (active)
			{
				active = false;
				dialogueText.HideCanvas();
			}
			else
			{
				active = true;
				dialogueText.ShowDialogue(textContent);
			}
		}

		public void OnInputRecieved()
		{
			dialogueText.HideCanvas();
			active = false;
		}

		public abstract void EnableTarget();
		public abstract void DisableTarget();
		public abstract void YesResponse();
		public abstract void NoResponse();
	}
}
