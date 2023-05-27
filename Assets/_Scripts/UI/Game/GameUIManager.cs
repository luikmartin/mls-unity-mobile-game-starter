using TMPro;
using UnityEngine;

public class GameUIManager : UIManager
{
	private static readonly string FINAL_SCORE_TEMPLATE_KEY = "gameScene.gameOverView.finalScoreTemplate";

	[SerializeField]
	private TextMeshProUGUI _dayText;

	[Space(10)]
	[SerializeField]
	private TextMeshProUGUI _scoreText;
	[SerializeField]
	private TextMeshProUGUI _gameOverScoreText;

	private Animator _animator;

	private GameManager _gameManager;


	public override void Awake()
	{
		base.Awake();

		_animator = GetComponent<Animator>();

		_gameManager = GameManager.Instance;
	}

	public void Pause()
	{
		_gameManager.Pause();

		SetViewActive(Constants.PAUSE_VIEW);
	}

	public void Resume()
	{
		_gameManager.Resume();

		SetViewActive(Constants.GAME_VIEW);
	}

	public void Restart()
	{
		_gameManager.Restart();
	}

	public void Quit()
	{
		LoadMenuScene();
	}

	public void OpenGameOverView(int score)
	{
		_gameOverScoreText.text = Localization.Instance.GetText(FINAL_SCORE_TEMPLATE_KEY, score);

		SetViewActive(Constants.GAME_OVER_VIEW);
	}

	public void SetScore(int amount) => _scoreText.text = amount.ToString();

	public void LoadNextDay(int day)
	{
		_dayText.text = "Day " + day;
		_animator.Play("LoadNextDay", 0, 0);
	}

	public void Reset() => GameManager.Instance.Reset();

	public void InitGame() => GameManager.Instance.InitGame();
}
