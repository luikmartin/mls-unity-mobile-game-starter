using NUnit.Framework;
using UnityEngine;
using NSubstitute;

public class GameControllerTests
{
	GameController gameController;
	IGameUIController gameUIControllerMock;

	[SetUp]
	public void SetUp()
	{
		Debug.Log("SetUp() runs before each test");

		gameController = new GameObject().AddComponent<GameController>();

		gameUIControllerMock = Substitute.For<IGameUIController>();
		gameUIControllerMock.When(x => x.SetScore(Arg.Any<int>()));
		gameController.gameUIController = gameUIControllerMock;
	}

	[Test]
	public void GivenGameController_WhenAddingToScore_ThenScoreIsUpdated()
	{
		gameController.AddToScore(1);
		Assert.AreEqual(gameController.Score, 1);
	}

	[Test]
	public void GivenGameController_WhenSubstractingFromScore_ThenScoreIsUpdated()
	{
		gameController.AddToScore(1);
		Assert.AreEqual(gameController.Score, 1);

		gameController.SubtractFromScore(1);
		Assert.AreEqual(gameController.Score, 0);
	}

	[Test]
	public void GivenGameController_WhenSubstractingMoreThanCurrentScore_ThenScoreDoesNotGoBelowZero()
	{
		gameController.AddToScore(1);
		Assert.AreEqual(gameController.Score, 1);

		gameController.SubtractFromScore(2);
		Assert.AreEqual(gameController.Score, 0);
	}

	[TearDown]
	public void TearDown()
	{
		Debug.Log("TearDown() runs after each test");
	}
}
