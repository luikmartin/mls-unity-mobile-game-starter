using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Score { get; private set; }

    private GameUIController _gameUIController;

    private void Awake()
    {
        _gameUIController = GameObject.FindObjectOfType<GameUIController>();
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {

    }

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

    public void GameOver()
    {
        _gameUIController.OpenGameOverView(Score);
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
}
