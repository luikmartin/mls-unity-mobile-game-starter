using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIController : UIController
{
	private static readonly string HIGH_SCORE_TEMPLATE_KEY = "menuScene.statsView.highScoreTemplate";
	private static readonly string RESET_PROFILE_MODAL_TITLE_KEY = "menuScene.settingsView.resetProfile.modal.title";
	private static readonly string RESET_PROFILE_MODAL_MESSAGE_KEY = "menuScene.settingsView.resetProfile.modal.message";
	private static readonly string RESET_PROFILE_MODAL_CONFIRM_BUTTON_KEY = "menuScene.settingsView.resetProfile.modal.confirmButton";

	[SerializeField]
	private TextMeshProUGUI _highScoreField;
	[Space(10)]
	[SerializeField]
	private ScrollRect _scrollRect;
	[SerializeField]
	private Button _scrollToTopButton;
	[SerializeField]
	private float _threshold = 0.9f;


	private void Start()
	{
		var highScore = Saves.Instance.saveFile.highScore;
		_highScoreField.text = Localization.Instance.GetText(HIGH_SCORE_TEMPLATE_KEY, highScore);

		_scrollRect.onValueChanged.AddListener(OnScroll);
	}

	public void OpenGameView() => _scenesController.LoadGameScene();

	public void OpenStatsView() => SetViewActive(Constants.STATS_VIEW);

	public void OpenSettingsView() => SetViewActive(Constants.SETTINGS_VIEW);

	public void OpenMenuView()
	{
		_scrollRect.verticalNormalizedPosition = 1;
		_scrollToTopButton.gameObject.SetActive(false);

		SetViewActive(Constants.MENU_VIEW);
	}

	public void OpenAchievementsView() => SetViewActive(Constants.ACHIEVEMENTS_VIEW);

	public void OnLanguageValueChange(int value)
	{
		Localization.Instance.SetLanguage((Language)value);
	}

	public void ResetPlayerProfile()
	{
		Modal.Instance.Open(new ModalConfig
		{
			title = Localization.Instance.GetText(RESET_PROFILE_MODAL_TITLE_KEY),
			message = Localization.Instance.GetText(RESET_PROFILE_MODAL_MESSAGE_KEY),
			confirmButtonLabel = Localization.Instance.GetText(RESET_PROFILE_MODAL_CONFIRM_BUTTON_KEY)
		});
	}

	public void ConfirmResetPlayerProfile()
	{
		Modal.Instance.Close();

		Saves.Instance.Create();
	}

	private void OnScroll(Vector2 position) => _scrollToTopButton.gameObject.SetActive(position.y <= _threshold);
}
