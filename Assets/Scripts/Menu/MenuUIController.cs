public class MenuUIController : UIController
{
    public void OpenGameView() => _scenesController.LoadGameScene();

    public void OpenStatsView() => SetViewActive(Constants.STATS_VIEW);

    public void OpenSettingsView() => SetViewActive(Constants.SETTINGS_VIEW);

    public void OpenMenuView() => SetViewActive(Constants.MENU_VIEW);

    public void OnLanguageValueChange(int value) => Localization.Instance.SetLanguage((Language)value);
}
