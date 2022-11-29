using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

namespace DuneRiders {
	[RequireComponent(typeof(HandCanvasPointer))]
	public class UIPointerEventProxy : MonoBehaviour
	{
		HandCanvasPointer handCanvasPointer;
		float initialRaycastLength;

		void Awake() {
			handCanvasPointer = GetComponent<HandCanvasPointer>();
		}

		public void Press() {
			if (gameObject.activeSelf) handCanvasPointer.Press();
		}

		public void Release() {
			if (gameObject.activeSelf) handCanvasPointer.Release();
		}
	}
}
