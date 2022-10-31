using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DuneRiders {
	public class BubbleGameObjectToActiveScene
	{
		public static void BubbleUp(GameObject gameObject) {
			gameObject.transform.SetParent(null);
			SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
		}
	}
}
