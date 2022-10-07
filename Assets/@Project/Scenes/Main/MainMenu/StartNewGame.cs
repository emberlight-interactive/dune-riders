using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	public class StartNewGame : MonoBehaviour
	{
		[SerializeField] List<string> saveFiles = new List<string>();

		public void SetSaveFile() {
			foreach (var saveFile in saveFiles) {
				if (!ES3.FileExists(saveFile)) {
					UnityEngine.Object.FindObjectOfType<GameManager>().persistenceFileName = saveFile;
					return;
				}
			}

			Debug.Log("No save files");
		}
	}
}
