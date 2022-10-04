using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuneRiders.VillageMigrationSystem {
	[RequireComponent(typeof(UniqueIdentifier))]
	public class VillageMigrationWaypoint : MonoBehaviour
	{
		public VillageMigrationWaypoint nextWaypoint;

		#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			Color sphereColor = Color.magenta;
			Handles.color = sphereColor;

			GUIStyle style = new GUIStyle();
			style.normal.textColor = sphereColor;

			Handles.Label(transform.position, gameObject.name, style);
			Gizmos.DrawSphere(transform.position, 1);
		}
		#endif
	}
}
