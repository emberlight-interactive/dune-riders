using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DuneRiders {
	public class LoadSlot : MonoBehaviour
	{
		[SerializeField] SaveFileOptions saveFileOptions;
		[SerializeField] GameObject emptySlotIndicator;
		[SerializeField] Button loadSaveButton;
		[SerializeField] TextMeshProUGUI loadSaveButtonText;
		[SerializeField] string saveFile;
		[SerializeField] string saveFileTitle;

		void OnEnable() {
			loadSaveButtonText.text = saveFileTitle;

			if (ES3.FileExists(saveFile)) {
				ShowSaveButton();
				loadSaveButton.onClick.AddListener(() => InitSaveFileOptionsScreen());
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

		void InitSaveFileOptionsScreen() {
			saveFileOptions.SaveFileName = saveFile;
			saveFileOptions.SaveFileTitle = saveFileTitle;
		}
	}
}
