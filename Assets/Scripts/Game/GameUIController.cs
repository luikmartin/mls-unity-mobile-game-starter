using TMPro;
using UnityEngine;

public class GameUIController : UIController
{
    private static readonly string FINAL_SCORE_TEMPLATE = "gameScene.gameOverView.finalScoreTemplate";

    [SerializeField]
    private TextMeshProUGUI _scoreField;
    [Space(10)]
    [SerializeField]
    private TextMeshProUGUI _gameOverScoreField;

    public void OpenGameView() => SetViewActive(Constants.GAME_VIEW);

    public void OpenPauseView() => SetViewActive(Constants.PAUSE_VIEW);

    public void OpenGameOverView(int score)
    {
        var finalScoreTemplate = Localization.Instance.GetText(FINAL_SCORE_TEMPLATE);
        _gameOverScoreField.text = string.Format(finalScoreTemplate, score);

        SetViewActive(Constants.GAME_OVER_VIEW);
    }

    public void QuitToMainMenu() => _scenesController.LoadMenuScene();

    public void SetScore(int amount) => _scoreField.text = amount.ToString();
}
