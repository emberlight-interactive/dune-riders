using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System;

public class DialogueTarget : MonoBehaviour
{
	//This can be replaced by a scriptable object system after prototype
	[BoxGroup("Variables"), TextArea, SerializeField] private string textContent;

	[BoxGroup("Components"), SerializeField] private DialogueText dialogueText;
	[BoxGroup("Components"), SerializeField] private TextMeshPro textMesh;

	private bool active;

	private void Start()
	{
		textMesh.color = new Color(1, .5f, 0);
	}

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

	public void EnableTarget()
	{
		textMesh.color = Color.green;
	}

	public void DisableTarget()
	{
		textMesh.color = new Color(1, .5f, 0);
	}
}
