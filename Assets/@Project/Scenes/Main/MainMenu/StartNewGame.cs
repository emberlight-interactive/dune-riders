using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace DuneRiders {
	public class StartNewGame : MonoBehaviour
	{
		[SerializeField] List<string> saveFiles = new List<string>();

		public void StartNewGameCallback() {
			foreach (var saveFile in saveFiles) {
				if (!ES3.FileExists(saveFile)) {
					ES3Settings.defaultSettings.path = saveFile;
					SceneManager.LoadScene("Main");

					return;
				}
			}

			Debug.Log("No save files");
		}
	}
}
