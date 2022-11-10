using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.NPCGuidanceSystem {
	[RequireComponent(typeof(ParvTutorialTips))]
	public class PlayMigrationTipAfterDelay : MonoBehaviour
	{
		[SerializeField] float delay = 20f;

		public void PlayTip() {
			StartCoroutine(AfterDelay());
		}

		IEnumerator AfterDelay() {
			yield return new WaitForSeconds(delay);
			GetComponent<ParvTutorialTips>().PlayMigrationTip();
		}
	}
}
