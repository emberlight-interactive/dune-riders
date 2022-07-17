using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using DuneRiders.YesNoSystem;
using DuneRiders.GatheringSystem;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.MercenaryHireSystem {
	public class MercenaryHireDialogueTarget : DialogueTarget
	{
		[BoxGroup("Components"), SerializeField] private TextMeshPro textMesh;
		[SerializeField] Gatherer gatherer;
		[SerializeField] Transform dummyRider;
		[SerializeField] Rider riderToSpawn;
		[SerializeField] GameObject mercenaryInteractable;

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
			if (gatherer.GetPreciousMetal(35)) {
				dummyRider.gameObject.SetActive(false);
				SimplePool.Spawn(riderToSpawn.gameObject, dummyRider.position, dummyRider.rotation);
				mercenaryInteractable.SetActive(false);
			}
		}
		public override void NoResponse() {
			Debug.Log("The outcome was: NO");
		}
	}
}
