using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DuneRiders {
	public class SceneTransitioner : MonoBehaviour
	{
		[SerializeField] bool loadAdditively = false;
		[SerializeField] string sceneToLoad;

		public void LoadNextScene() {
			SceneManager.LoadScene(sceneToLoad, loadAdditively ? LoadSceneMode.Additive : LoadSceneMode.Single);
		}

		public bool manuallyTransitionSceneButton = false;

		void OnValidate() {
			if (manuallyTransitionSceneButton) {
				manuallyTransitionSceneButton = false;
				LoadNextScene();
			}
		}
	}
}
