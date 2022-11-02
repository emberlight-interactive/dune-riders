using System;
using System.Collections;
using System.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.SceneManagement;

namespace DuneRiders {
	public class SceneLoadRedirect : MonoBehaviour
	{
		SceneLoadRedirectDTO sceneLoadRedirectDTO;

		void Awake() {
			sceneLoadRedirectDTO = FindObjectOfType<SceneLoadRedirectDTO>();
			ChangeSceneSkyBox();
			RenderNothingButSkybox();
		}

		void Start() {
			var unloadTask = UnloadAllScenesExcept(SceneManager.GetActiveScene().name);
			if (unloadTask == null) {
				LoadNextScene();
			} else {
				unloadTask.completed += (operation) => {
					LoadNextScene();
				};
			}
		}

		void ChangeSceneSkyBox() {
			RenderSettings.skybox = sceneLoadRedirectDTO.loadingSkybox;
		}

		void RenderNothingButSkybox() {
			var cameras = FindObjectsOfType<Camera>();
			foreach(var camera in cameras) {
				camera.cullingMask = 0;
			}
		}

		AsyncOperation UnloadAllScenesExcept(string sceneName) {
			int c = SceneManager.sceneCount;
			AsyncOperation unloadOperation = null;

			for (int i = 0; i < c; i++) {
				Scene scene = SceneManager.GetSceneAt(i);
				if (scene.name != sceneName) {
					unloadOperation = SceneManager.UnloadSceneAsync(scene);
				}
			}

			return unloadOperation;
		}

		void LoadNextScene() {
			var unloadingOperation = Resources.UnloadUnusedAssets();
			unloadingOperation.completed += (operation) => {
				RunGC();

				var sceneToLoad = sceneLoadRedirectDTO.sceneToLoad;
				DestroyImmediate(sceneLoadRedirectDTO);

				SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
			};
		}

		void RunGC() {
			GarbageCollector.GCMode = GarbageCollector.Mode.Manual;
			GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
		}
	}

	public class SceneLoadRedirectDTO : MonoBehaviour {
		private static SceneLoadRedirectDTO instance;
		public static SceneLoadRedirectDTO Instance { get { return instance; } }

		public Material loadingSkybox;
		public string sceneToLoad;

		void Awake() {
			if (instance != null && instance != this) {
				Destroy(this.gameObject);
			} else {
				instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
		}
	}
}
