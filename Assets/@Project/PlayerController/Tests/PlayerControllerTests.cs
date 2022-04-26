using DuneRiders.Shared;

//
public class PlayerControllerTests : SnapshotTest
{
	protected override int ScreenWidth { get => 700; }
	protected override int ScreenHeight { get => 500; }
	protected override string SceneFilePath { get => "Assets/@Project/PlayerController/PlayerControllerTest.unity"; }
	protected override int PhysicsLoopsToCover { get => 300; }
	protected override int NumberOfScreenshotsToTake { get => 2; }
	protected override string[] ExpectedSeriesOfActionsInTest {
		get => new string[] {"Clench hands", "Teleport", "Snapturn", "Glide across floor"};
	}
	protected override string DirectoryToStoreScreenShots { get => "Assets/@Project/PlayerController/Tests/Screenshots/"; }
}
