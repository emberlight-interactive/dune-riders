using System.Diagnostics;
using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEditor.TestTools;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace DuneRiders.Shared {
	public class SnapshotTestMeta {
		public static string SettingName = "AreSnapshotTestsInRecordMode";
	}

	public class SnapshotTestInfoWindow : EditorWindow {
		private string[] _expectedSeriesOfActionsInTest;
		private string _windowTitle;

		public SnapshotTestInfoWindow(string[] expectedSeriesOfActionsInTest, string windowTitle) {
			this._expectedSeriesOfActionsInTest = expectedSeriesOfActionsInTest;
			this._windowTitle = windowTitle;
		}

        void OnGUI()
        {
            titleContent = new GUIContent(_windowTitle);

			foreach(string step in _expectedSeriesOfActionsInTest)
            {
                GUILayout.Label($"- {step}", EditorStyles.largeLabel);
            }
        }
    }

	public class SnapShotTaker : MonoBehaviour
	{
		const string FileFormat = ".tmp.png";
		public bool RecordMode;
		public string DirectoryToStoreScreenShots;
		public int PhysicsLoopsToCover;
		public int NumberOfScreenshotsToTake;
		public string TestClassName;
		public int ScreenWidth;
		public int ScreenHeight;
		int FixedStepCounter;
		int CurrentScreenShot = 0;
		bool takeScreenShot = false;

		void Awake() {
			if (RecordMode)
			{
				SetupDirectoryIfNeeded();
			}

			Camera.onPostRender += OnPostRenderCallback;
		}

		void OnPostRenderCallback(Camera cam)
    	{
			if (takeScreenShot) {
				TakeScreenshot($"Shot-{CurrentScreenShot}");
				UnityEngine.Debug.Log($"Shot {CurrentScreenShot} good");
				takeScreenShot = false;
				Time.timeScale = 1;
			}
		}

		void FixedUpdate() {
			FixedStepCounter++;
			if (FixedStepCounter % Mathf.Floor(PhysicsLoopsToCover / NumberOfScreenshotsToTake) == 0) {
				takeScreenShot = true;
				CurrentScreenShot++;
				Time.timeScale = 0;
			}

		}

		void OnDestroy()
		{
			Camera.onPostRender -= OnPostRenderCallback;
		}

		private void SetupDirectoryIfNeeded()
		{

			if (!System.IO.Directory.Exists(DirectoryToStoreScreenShots))
			{
				System.IO.Directory.CreateDirectory(DirectoryToStoreScreenShots);
			}
		}

		private void TakeScreenshot(string identifier = null) {
			var filePath = GetFilePath(identifier);
			var imageBytes = ScreenCaptureInBytes();
			var existingImageInBytes = GetFileIfExists(filePath);

			#if UNITY_EDITOR
			System.IO.File.WriteAllBytes(filePath, imageBytes);
			AssetDatabase.Refresh();
			#endif

			UnityEngine.Debug.Log(FixedStepCounter);
		}

		private byte[] ScreenCaptureInBytes()
		{
			Texture2D screenImage = new Texture2D(ScreenWidth, ScreenHeight);
			//Get Image from screen
			screenImage.ReadPixels(new Rect(0, 0, ScreenWidth, ScreenHeight), 0, 0);
			screenImage.Apply();
			//Convert to png
			return screenImage.EncodeToPNG();
		}

		private byte[] GetFileIfExists(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}

			return File.ReadAllBytes(path);
		}

		private string GetFilePath(string suffix = null)
		{
			var nameSuffix = "";
			if (suffix != null)
			{
				nameSuffix = "_" + suffix;
			}
			return $"{DirectoryToStoreScreenShots}/{TestClassName}{nameSuffix}{FileFormat}";
		}
	}

	public abstract class SnapshotTest
	{
		const string FileFormat = ".png";

		protected bool RecordMode = EditorPrefs.GetBool(SnapshotTestMeta.SettingName, false);
		abstract protected int ScreenWidth { get; }
		abstract protected int ScreenHeight { get; }
		abstract protected string SceneFilePath { get; }
		abstract protected int PhysicsLoopsToCover { get; }
		abstract protected int NumberOfScreenshotsToTake { get; }
		abstract protected string[] ExpectedSeriesOfActionsInTest { get; }
		/// <summary>
		/// End path with a slash
		/// </summary>
		abstract protected string DirectoryToStoreScreenShots { get; }

		string _testClassName = "Test";

		SnapshotTestInfoWindow _window;

		[UnitySetUp]
		protected IEnumerator Setup()
		{
			SetupCurrentClassName();

			_window = new SnapshotTestInfoWindow(ExpectedSeriesOfActionsInTest, _testClassName);
			EditorWindow.GetWindow<SnapshotTestInfoWindow>(_window).Show();

			if (RecordMode)
			{
				SetupDirectoryIfNeeded();
			}

			// Application.targetFrameRate = 50;

			EditorSceneManager.sceneLoaded += SetUpScreenShotter;
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(SceneFilePath, new LoadSceneParameters(LoadSceneMode.Single));
		}

		private void SetUpScreenShotter(Scene scene, LoadSceneMode loadSceneMode) {
			var gm = new GameObject();
			var comp = gm.AddComponent<SnapShotTaker>();

			comp.RecordMode = RecordMode;
			comp.DirectoryToStoreScreenShots = DirectoryToStoreScreenShots;
			comp.PhysicsLoopsToCover = PhysicsLoopsToCover;
			comp.NumberOfScreenshotsToTake = NumberOfScreenshotsToTake;
			comp.TestClassName = _testClassName;
			comp.ScreenWidth = ScreenWidth;
			comp.ScreenHeight = ScreenHeight;
		}

		[UnityTest]
        public IEnumerator ScreenshotTest()
        {
			// int currentScreenShot = 1;
            // for (int i = 0; i < PhysicsLoopsToCover; i++)
            // {
            //     yield return new WaitForFixedUpdate();
            //     if (i % Mathf.Floor(PhysicsLoopsToCover / NumberOfScreenshotsToTake) == 0) {
            //         yield return new WaitForEndOfFrame();
            //         SnapshotVerifyView($"Shot-{currentScreenShot}");
            //         UnityEngine.Debug.Log($"Shot {currentScreenShot} good");
			// 		currentScreenShot++;
            //     }
            // }

			yield return new WaitForSeconds(3.0f);
            _window.Close();
        }

		protected void SnapshotVerifyView(string identifier = null)
		{
			var filePath = GetFilePath(identifier);
			var imageBytes = ScreenCaptureInBytes();
			var existingImageInBytes = GetFileIfExists(filePath);

			if (RecordMode) {
				#if UNITY_EDITOR
				System.IO.File.WriteAllBytes(filePath, imageBytes);
				AssetDatabase.Refresh();
				#endif
				// Assert.Pass("Successfully saved screenshot");
			} else if (existingImageInBytes == null) {
				Assert.Fail("Image does not exist");
			} else {
				Assert.AreEqual(existingImageInBytes, imageBytes);
			}
		}

		private void SetupDirectoryIfNeeded()
		{

			if (!System.IO.Directory.Exists(DirectoryToStoreScreenShots))
			{
				System.IO.Directory.CreateDirectory(DirectoryToStoreScreenShots);
			}
		}

		private byte[] ScreenCaptureInBytes()
		{
			Texture2D screenImage = new Texture2D(ScreenWidth, ScreenHeight);
			//Get Image from screen
			screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
			screenImage.Apply();
			//Convert to png
			return screenImage.EncodeToPNG();
		}

		private byte[] GetFileIfExists(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}

			return File.ReadAllBytes(path);
		}

		private string GetFilePath(string suffix = null)
		{
			var nameSuffix = "";
			if (suffix != null)
			{
				nameSuffix = "_" + suffix;
			}
			return $"{DirectoryToStoreScreenShots}/{_testClassName}{nameSuffix}{FileFormat}";
		}

		private void SetupCurrentClassName()
		{
			_testClassName = GetType().Name;
		}
	}

	public class SnapshotTestSettings : MonoBehaviour
	{
		private const string MenuName = "Emberlight Interactive/Snapshot Test Settings/Enable Recording Mode";

		public static bool IsEnabled
		{
			get { return EditorPrefs.GetBool(SnapshotTestMeta.SettingName, false); }
			set { EditorPrefs.SetBool(SnapshotTestMeta.SettingName, value); }
		}

		[MenuItem(MenuName)]
		private static void EnableRecordingMode()
		{
			IsEnabled = !IsEnabled;
		}

		[MenuItem(MenuName, true)]
		private static bool EnableRecordingModeValidate()
		{
			Menu.SetChecked(MenuName, IsEnabled);
			return true;
		}
	}
}
