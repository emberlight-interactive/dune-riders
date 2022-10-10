using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DuneRiders {
	public class StartNewGame : MonoBehaviour
	{
		[SerializeField] List<string> saveFiles = new List<string>();
		[SerializeField] TextMeshProUGUI noFreeSaveSlotsText;
		[SerializeField] SceneTransitioner openingCinematicSceneTransitioner;

		bool SetSaveFile() {
			foreach (var saveFile in saveFiles) {
				if (!ES3.FileExists(saveFile)) {
					UnityEngine.Object.FindObjectOfType<GameManager>().persistenceFileName = saveFile;
					return true;
				}
			}

			noFreeSaveSlotsText.transform.gameObject.SetActive(true);
			return false;
		}

		public void LaunchGame() {
			if (SetSaveFile()) openingCinematicSceneTransitioner.LoadNextScene();
		}
	}
}
