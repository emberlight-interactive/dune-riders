using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DuneRiders {
	public class LoadSlot : MonoBehaviour
	{
		[SerializeField] GameObject emptySlotIndicator;
		[SerializeField] Button loadSaveButton;
		[SerializeField] string saveFile;

		void Awake() {
			if (ES3.FileExists(saveFile)) {
				ShowSaveButton();
				loadSaveButton.onClick.AddListener(() => SetLoadFileAndStartScene());
			} else {
				HideSaveButton();
			}
		}

		public void ShowSaveButton() {
			emptySlotIndicator.SetActive(false);
			loadSaveButton.gameObject.SetActive(true);
		}

		public void HideSaveButton() {
			emptySlotIndicator.SetActive(true);
			loadSaveButton.gameObject.SetActive(false);
		}

		void SetLoadFileAndStartScene() {
			ES3Settings.defaultSettings.path = saveFile;
			SceneManager.LoadScene("Main");
		}
	}
}
