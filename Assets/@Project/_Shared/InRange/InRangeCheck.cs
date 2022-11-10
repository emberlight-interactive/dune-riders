using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace DuneRiders.Shared.InRange {
	public class InRangeCheck : MonoBehaviour
	{
		[SerializeField] string label = "RangeCheck";
		[SerializeField] float labelOffset = 3f;
		[SerializeField] float secondsBetweenChecks = 5f;
		[SerializeField] float maxDistanceForCheck = 600f;
		[SerializeField] List<string> tagsToCheckFor = new List<string>();
		[SerializeField] bool selfDestructAfterCheck = false;
		[SerializeField] UnityEvent tagInRangeEvent;

		void Start() {
			InvokeRepeating(nameof(CheckForTagsInRange), 1f, secondsBetweenChecks);
		}

		void CheckForTagsInRange() {
			var tags = FindObjectsOfType<Tag>();
			if (tags == null || tags.Length == 0) return;

			foreach (var tag in tags) {
				if (Vector3.Distance(tag.transform.position, transform.position) <= maxDistanceForCheck) {
					foreach (var tagToCheck in tagsToCheckFor) {
						if (tag.tagString == tagToCheck) {
							tagInRangeEvent?.Invoke();
							if (selfDestructAfterCheck) Destroy(this);
						}
					}
				}
			}
		}

		#if UNITY_EDITOR
		void OnDrawGizmos() {
			DrawRange(Color.white, label, maxDistanceForCheck);
		}

		void DrawRange(Color color, string label, float rangeDistance) {
			GUIStyle style = new GUIStyle();

			var labelPosition = transform.position;

			Handles.color = color;
			style.normal.textColor = color;
			labelPosition.y += labelOffset;
			Handles.Label(labelPosition, label, style);
			Handles.DrawWireDisc(transform.position, new Vector3(0, 1, 0), rangeDistance);
		}
		#endif
	}

	public abstract class Tag : MonoBehaviour
	{
		public abstract string tagString { get; }
	}
}
