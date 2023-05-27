using NUnit.Framework;
using UnityEngine;
using NSubstitute;

public class GameControllerTests
{
	GameManager gameManager;
	GameUIManager gameUIManagerMock;

	[SetUp]
	public void SetUp()
	{
		Debug.Log("SetUp() runs before each test");

		gameManager = new GameObject().AddComponent<GameManager>();

		gameUIManagerMock = Substitute.For<GameUIManager>();
		gameUIManagerMock.When(x => x.SetScore(Arg.Any<int>()));
	}

	[Test]
	public void GivenGameController_WhenAddingToScore_ThenScoreIsUpdated()
	{
		// gameManager.AddToScore(1);
		// Assert.AreEqual(gameManager.Score, 1);
	}

	[Test]
	public void GivenGameController_WhenSubstractingFromScore_ThenScoreIsUpdated()
	{
		// gameManager.AddToScore(1);
		// Assert.AreEqual(gameManager.Score, 1);

		// gameManager.SubtractFromScore(1);
		// Assert.AreEqual(gameManager.Score, 0);
	}

	[Test]
	public void GivenGameController_WhenSubstractingMoreThanCurrentScore_ThenScoreDoesNotGoBelowZero()
	{
		// gameManager.AddToScore(1);
		// Assert.AreEqual(gameManager.Score, 1);

		// gameManager.SubtractFromScore(2);
		// Assert.AreEqual(gameManager.Score, 0);
	}

	[TearDown]
	public void TearDown()
	{
		Debug.Log("TearDown() runs after each test");
	}
}
