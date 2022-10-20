using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DuneRiders {
	public class SceneTransitioner : MonoBehaviour
	{
		[SerializeField] bool loadAdditively = false;
		[SerializeField] string sceneToLoad;
		[SerializeField] Material loadingSkyBox;

		public void LoadNextScene() {
			ChangeSceneSkyBox();
			RenderNothingButSkybox();
			SceneManager.LoadSceneAsync(sceneToLoad, loadAdditively ? LoadSceneMode.Additive : LoadSceneMode.Single);
		}

		public bool manuallyTransitionSceneButton = false;

		void OnValidate() {
			if (manuallyTransitionSceneButton) {
				manuallyTransitionSceneButton = false;
				LoadNextScene();
			}
		}

		void ChangeSceneSkyBox() {
			RenderSettings.skybox = loadingSkyBox;
		}

		void RenderNothingButSkybox() {
			var cameras = FindObjectsOfType<Camera>();
			foreach(var camera in cameras) {
				camera.cullingMask = 0;
			}
		}
	}
}
