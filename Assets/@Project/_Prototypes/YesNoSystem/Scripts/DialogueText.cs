using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueText : MonoBehaviour
{
	[BoxGroup("Components"), SerializeField] private TextMeshProUGUI mainText;
	[BoxGroup("Components"), SerializeField] private CanvasGroup canvasGroup;

	private bool busy = false;

	public void ShowDialogue(string text)
	{
		if (busy == false)
		{
			mainText.text = text;
			busy = true;
			canvasGroup.DOFade(1, .1f).onComplete += () =>
			{
				busy = false;
			};
		}
	}

	public void HideCanvas()
	{
		if (busy == false)
		{
			mainText.text = "";
			busy = true;
			canvasGroup.DOFade(0, .1f).onComplete += () => busy = false;
		}
	}
}
