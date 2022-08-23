using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace DuneRiders.CommandHonkSystem {
	public class CommandSelector : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI currentlySelectedCommandDisplay;
		[SerializeField] InputActionProperty selectorToggle;
		[SerializeField] List<Command> commands = new List<Command>();
		int currentCommandIndex = 0;
		bool selected = false;
		public Command CurrentCommand { get; private set;}

		void Start() {
			selectorToggle.action.Enable();

			if (commands.Count > 0) {
				CurrentCommand = commands[currentCommandIndex];
			}
		}
		void Update() {
			if (!selected && (SelectorToggleX() > 0.2f || SelectorToggleY() > 0.2f)) {
				SelectNextCommand();
				selected = true;
			} else if (!selected && (SelectorToggleX() < -0.2f || SelectorToggleY() < -0.2f)) {
				SelectPreviousCommand();
				selected = true;
			}

			if (selected && Mathf.Abs(SelectorToggleX()) < 0.2f && Mathf.Abs(SelectorToggleY()) < 0.2f) selected = false;
		}

		float SelectorToggleY() {
			return selectorToggle.action.ReadValue<Vector2>().y;
		}

		float SelectorToggleX() {
			return selectorToggle.action.ReadValue<Vector2>().x;
		}

		void SelectNextCommand() {
			if ((currentCommandIndex + 1) == commands.Count) {
				currentCommandIndex = 0;
			} else {
				currentCommandIndex++;
			}

			CurrentCommand = commands[currentCommandIndex];
			UpdateDisplay();
		}

		void SelectPreviousCommand() {
			if ((currentCommandIndex - 1) == -1) {
				currentCommandIndex = commands.Count - 1;
			} else {
				currentCommandIndex--;
			}

			CurrentCommand = commands[currentCommandIndex];
			UpdateDisplay();
		}

		void UpdateDisplay() {
			currentlySelectedCommandDisplay.text = CurrentCommand.commandName;
		}
	}
}
