using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DuneRiders {
	public class SaveFileOptions : MonoBehaviour
	{
		GameManager gameManager;
		[SerializeField] DeleteSave deleteSave;
		[SerializeField] SceneTransitioner mainSceneTransitioner;
		[SerializeField] TextMeshProUGUI saveTitleText;
		public string SaveFileName { get; set; }
		string saveFileTitle;
		public string SaveFileTitle {
			get => saveFileTitle;
			set {
				saveFileTitle = value;
				saveTitleText.text = value;
			}
		}

		void Awake() {
			gameManager = FindObjectOfType<GameManager>();
		}

		public void LoadGame() {
			gameManager.persistenceFileName = SaveFileName;
			mainSceneTransitioner.LoadNextScene();
		}

		public void InitDeleteSavePanel() {
			deleteSave.SaveFileName = SaveFileName;
		}
	}
}
