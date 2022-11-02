using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DuneRiders {
	public class SceneTransitioner : MonoBehaviour
	{
		[SerializeField] bool loadAdditively = false;
		[SerializeField] bool useRedirectionScene = false;
		[SerializeField] bool useHeavyUnload = false;
		[SerializeField] string sceneToLoad;
		[SerializeField] Material loadingSkyBox;
		string sceneLoadRedirectSceneName = "SceneLoadRedirect";

		public void LoadNextScene() {
			ChangeSceneSkyBox();
			RenderNothingButSkybox();

			if (useHeavyUnload) UnloadAllScenes();

			if (useRedirectionScene) LoadNextSceneWithRedirection();
			else {
				SceneManager.LoadScene(sceneToLoad, loadAdditively ? LoadSceneMode.Additive : LoadSceneMode.Single);
			}
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

		void LoadNextSceneWithRedirection() {
			var sceneLoadRedirectDTO = new GameObject();
			BubbleGameObjectToActiveScene.BubbleUp(sceneLoadRedirectDTO);
			var dto = sceneLoadRedirectDTO.AddComponent<SceneLoadRedirectDTO>();
			dto.sceneToLoad = this.sceneToLoad;
			dto.loadingSkybox = this.loadingSkyBox;
			SceneManager.LoadScene(sceneLoadRedirectSceneName, LoadSceneMode.Single);
		}

		void UnloadAllScenes() {
			int c = SceneManager.sceneCount;

			for (int i = 0; i < c; i++) {
				Scene scene = SceneManager.GetSceneAt(i);
				SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
			}
		}
	}
}
