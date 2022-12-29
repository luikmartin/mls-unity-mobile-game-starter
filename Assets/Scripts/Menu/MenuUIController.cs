using TMPro;
using UnityEngine;

public class MenuUIController : UIController
{
    private static readonly string HIGH_SCORE_TEMPLATE_KEY = "menuScene.statsView.highScoreTemplate";

    [SerializeField]
    private TextMeshProUGUI _highScoreField;


    private void Start()
    {
        var highScore = Saves.Instance.saveFile.highScore;
        _highScoreField.text = Localization.Instance.GetText(HIGH_SCORE_TEMPLATE_KEY, highScore);
    }

    public void OpenGameView() => _scenesController.LoadGameScene();

    public void OpenStatsView() => SetViewActive(Constants.STATS_VIEW);

    public void OpenSettingsView() => SetViewActive(Constants.SETTINGS_VIEW);

    public void OpenMenuView() => SetViewActive(Constants.MENU_VIEW);

    public void OnLanguageValueChange(int value) => Localization.Instance.SetLanguage((Language)value);
}
