using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace DuneRiders.YesNoSystem {
	public class TestDialogueTarget : DialogueTarget
	{
		[BoxGroup("Components"), SerializeField] private TextMeshPro textMesh;

		void Start()
		{
			textMesh.color = new Color(1, .5f, 0);
		}

		public override void EnableTarget()
		{
			textMesh.color = Color.green;
		}

		public override void DisableTarget()
		{
			textMesh.color = new Color(1, .5f, 0);
		}

		public override void YesResponse() {
			Debug.Log("The outcome was: YES");
		}
		public override void NoResponse() {
			Debug.Log("The outcome was: NO");
		}
	}
}
