using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

namespace DuneRiders.Shared {
	public class ControllerInputMonitorWindow : EditorWindow {
		public bool rightControllerTrigger = false;
		public bool rightControllerGrip = false;
		public bool rightControllerB = false;
		public bool rightControllerA = false;
		public bool rightControllerToggleClick = false;
		public bool rightControllerToggleRight = false;
		public bool rightControllerToggleDown = false;
		public bool rightControllerToggleLeft = false;
		public bool rightControllerToggleUp = false;

		public bool leftControllerTrigger = false;
		public bool leftControllerGrip = false;
		public bool leftControllerY = false;
		public bool leftControllerX = false;
		public bool leftControllerToggleClick = false;
		public bool leftControllerToggleRight = false;
		public bool leftControllerToggleDown = false;
		public bool leftControllerToggleLeft = false;
		public bool leftControllerToggleUp = false;

		Texture2D controllersImage;
		Texture2D controllersRightTriggerImage;
		Texture2D controllersRightGripImage;
		Texture2D controllersRightBImage;
		Texture2D controllersRightAImage;
		Texture2D controllersRightToggleClick;
		Texture2D controllersRightToggleRight;
		Texture2D controllersRightToggleDown;
		Texture2D controllersRightToggleLeft;
		Texture2D controllersRightToggleUp;

		Texture2D controllersLeftTriggerImage;
		Texture2D controllersLeftGripImage;
		Texture2D controllersLeftYImage;
		Texture2D controllersLeftXImage;
		Texture2D controllersLeftToggleClick;
		Texture2D controllersLeftToggleRight;
		Texture2D controllersLeftToggleDown;
		Texture2D controllersLeftToggleLeft;
		Texture2D controllersLeftToggleUp;

		void OnEnable() {
			controllersImage = Resources.Load<Texture2D>("oculus-controllers");
			controllersRightTriggerImage = Resources.Load<Texture2D>("oculus-controllers-right-trigger");
			controllersRightGripImage = Resources.Load<Texture2D>("oculus-controllers-right-grip");
			controllersRightBImage = Resources.Load<Texture2D>("oculus-controllers-b");
			controllersRightAImage = Resources.Load<Texture2D>("oculus-controllers-a");
			controllersRightToggleClick = Resources.Load<Texture2D>("oculus-controllers-right-toggle-click");
			controllersRightToggleRight = Resources.Load<Texture2D>("oculus-controllers-right-toggle-right");
			controllersRightToggleDown = Resources.Load<Texture2D>("oculus-controllers-right-toggle-down");
			controllersRightToggleLeft = Resources.Load<Texture2D>("oculus-controllers-right-toggle-left");
			controllersRightToggleUp = Resources.Load<Texture2D>("oculus-controllers-right-toggle-up");

			controllersLeftTriggerImage = Resources.Load<Texture2D>("oculus-controllers-left-trigger");
			controllersLeftGripImage = Resources.Load<Texture2D>("oculus-controllers-left-grip");
			controllersLeftYImage = Resources.Load<Texture2D>("oculus-controllers-Y");
			controllersLeftXImage = Resources.Load<Texture2D>("oculus-controllers-X");
			controllersLeftToggleClick = Resources.Load<Texture2D>("oculus-controllers-left-toggle-click");
			controllersLeftToggleRight = Resources.Load<Texture2D>("oculus-controllers-left-toggle-right");
			controllersLeftToggleDown = Resources.Load<Texture2D>("oculus-controllers-left-toggle-down");
			controllersLeftToggleLeft = Resources.Load<Texture2D>("oculus-controllers-left-toggle-left");
			controllersLeftToggleUp = Resources.Load<Texture2D>("oculus-controllers-left-toggle-up");
		}

		void OnGUI() {
			GUI.color = new Color32(255, 255, 255, 0);

			var height = 350;
			var width = 600;

			ControllerInputMonitorWindow window = GetWindow<ControllerInputMonitorWindow>();
			window.minSize = new Vector2(width, height);
			window.maxSize = window.minSize;

			EditorGUI.DrawPreviewTexture(new Rect(0, 0, width, height), controllersImage);

			if (rightControllerTrigger) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersRightTriggerImage);
			if (rightControllerGrip) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersRightGripImage);
			if (rightControllerB) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersRightBImage);
			if (rightControllerA) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersRightAImage);
			if (rightControllerToggleClick) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersRightToggleClick);
			if (rightControllerToggleRight) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersRightToggleRight);
			if (rightControllerToggleDown) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersRightToggleDown);
			if (rightControllerToggleLeft) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersRightToggleLeft);
			if (rightControllerToggleUp) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersRightToggleUp);

			if (leftControllerTrigger) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersLeftTriggerImage);
			if (leftControllerGrip) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersLeftGripImage);
			if (leftControllerY) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersLeftYImage);
			if (leftControllerX) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersLeftXImage);
			if (leftControllerToggleClick) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersLeftToggleClick);
			if (leftControllerToggleRight) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersLeftToggleRight);
			if (leftControllerToggleDown) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersLeftToggleDown);
			if (leftControllerToggleLeft) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersLeftToggleLeft);
			if (leftControllerToggleUp) EditorGUI.DrawTextureTransparent(new Rect(0, 0, width, height), controllersLeftToggleUp);
		}
    }

	public class ControllerInputMonitor : MonoBehaviour
	{
		ControllerInputMonitorWindow _window;

		[Header("Oculus Inputs")]
		public InputActionProperty rightToggleThumbstick;
		public InputActionProperty rightToggleThumbstickClick;
		public InputActionProperty rightControllerTrigger;
		public InputActionProperty rightControllerGrip;
		public InputActionProperty rightControllerB;
		public InputActionProperty rightControllerA;

		public InputActionProperty leftToggleThumbstick;
		public InputActionProperty leftToggleThumbstickClick;
		public InputActionProperty leftControllerTrigger;
		public InputActionProperty leftControllerGrip;
		public InputActionProperty leftControllerY;
		public InputActionProperty leftControllerX;

		void Start()
		{
			_window = ScriptableObject.CreateInstance<ControllerInputMonitorWindow>();
			EditorWindow.GetWindow<ControllerInputMonitorWindow>(_window).Show();

			rightToggleThumbstickClick.action.Enable();
			rightToggleThumbstick.action.Enable();
			leftToggleThumbstickClick.action.Enable();
			leftToggleThumbstick.action.Enable();
		}

		void FixedUpdate() {
			_window.rightControllerTrigger = rightControllerTrigger.action.ReadValue<float>() > 0 ? true : false;
			_window.rightControllerGrip = rightControllerGrip.action.ReadValue<float>() > 0 ? true : false;
			_window.rightControllerB = rightControllerB.action.ReadValue<float>() > 0 ? true : false;
			_window.rightControllerA = rightControllerA.action.ReadValue<float>() > 0 ? true : false;
			_window.rightControllerToggleClick = rightToggleThumbstickClick.action.ReadValue<float>() > 0 ? true : false;
			_window.rightControllerToggleRight = rightToggleThumbstick.action.ReadValue<Vector2>().x > 0.22f ? true : false;
			_window.rightControllerToggleLeft = rightToggleThumbstick.action.ReadValue<Vector2>().x < -0.22f ? true : false;
			_window.rightControllerToggleDown = rightToggleThumbstick.action.ReadValue<Vector2>().y < -0.22f ? true : false;
			_window.rightControllerToggleUp = rightToggleThumbstick.action.ReadValue<Vector2>().y > 0.22f ? true : false;

			_window.leftControllerTrigger = leftControllerTrigger.action.ReadValue<float>() > 0 ? true : false;
			_window.leftControllerGrip = leftControllerGrip.action.ReadValue<float>() > 0 ? true : false;
			_window.leftControllerY = leftControllerY.action.ReadValue<float>() > 0 ? true : false;
			_window.leftControllerX = leftControllerX.action.ReadValue<float>() > 0 ? true : false;
			_window.leftControllerToggleClick = leftToggleThumbstickClick.action.ReadValue<float>() > 0 ? true : false;
			_window.leftControllerToggleRight = leftToggleThumbstick.action.ReadValue<Vector2>().x > 0.22f ? true : false;
			_window.leftControllerToggleLeft = leftToggleThumbstick.action.ReadValue<Vector2>().x < -0.22f ? true : false;
			_window.leftControllerToggleDown = leftToggleThumbstick.action.ReadValue<Vector2>().y < -0.22f ? true : false;
			_window.leftControllerToggleUp = leftToggleThumbstick.action.ReadValue<Vector2>().y > 0.22f ? true : false;

			_window.Repaint();
		}

		void OnDestroy() {
			_window.Close();
		}
	}
}
