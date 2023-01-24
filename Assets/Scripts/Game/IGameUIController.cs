public interface IGameUIController : IUIController
{
	void OpenGameOverView(int score);
	void OpenGameView();
	void OpenPauseView();
	void QuitToMainMenu();
	void SetScore(int amount);
}
