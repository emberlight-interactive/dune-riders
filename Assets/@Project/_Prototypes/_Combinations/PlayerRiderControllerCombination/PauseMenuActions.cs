using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class PauseMenuActions : MonoBehaviour
	{
		// todo: when using airlink to save and quit it can break airlink when testing again by seizing the camera and hands
		public void SaveAndQuit() {
			#if UNITY_EDITOR
				EditorApplication.ExecuteMenuItem("Edit/Play");
			#else
				Application.Quit();
			#endif
		}
	}
}
