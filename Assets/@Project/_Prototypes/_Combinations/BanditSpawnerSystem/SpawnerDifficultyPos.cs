using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DuneRiders.BanditSpawnerSystem {
	public class SpawnerDifficultyPos : MonoBehaviour
	{
		public EnemyRiderAISpawner.SpawnDifficulty spawnDifficulty;

		#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			Color sphereColor = Color.gray;
			Handles.color = sphereColor;

			GUIStyle style = new GUIStyle();
			style.normal.textColor = sphereColor;

			Handles.Label(transform.position, gameObject.name, style);
			Gizmos.DrawSphere(transform.position, 1);
		}
		#endif
	}
}
