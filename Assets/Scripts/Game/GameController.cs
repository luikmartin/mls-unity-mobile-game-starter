using UnityEngine;

public class GameController : MonoBehaviour
{
	public int Score { get; private set; }

	private GameUIController _gameUIController;

	private void Awake() => _gameUIController = FindObjectOfType<GameUIController>();

	private void Start() => StartGame();

	public void StartGame() => Time.timeScale = 1;

	public void Pause()
	{
		_gameUIController.OpenPauseView();

		Time.timeScale = 0;
	}

	public void Resume()
	{
		_gameUIController.OpenGameView();

		Time.timeScale = 1;
	}

	public void EndGame()
	{
		_gameUIController.OpenGameOverView(Score);

		UpdateStatsAndSave();
	}

	public void AddToScore(int amount)
	{
		Score += amount;

		_gameUIController.SetScore(Score);
	}

	public void SubtractFromScore(int amount)
	{
		Score -= amount;

		if (Score < 0)
		{
			Score = 0;
		}
		_gameUIController.SetScore(Score);
	}

	private void UpdateStatsAndSave()
	{
		var saveFile = Saves.Instance.saveFile;
		var highestCore = saveFile.highScore;

		saveFile.highScore = highestCore >= Score ? highestCore : Score;

		Saves.Instance.Save();
	}
}
