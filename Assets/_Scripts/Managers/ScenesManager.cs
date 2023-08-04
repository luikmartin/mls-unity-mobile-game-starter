using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ScenesManager : Singleton<ScenesManager>
{
	private const string LOAD_ANIMATION = "Load";
	private const string LOADED_ANIMATION = "Loaded";

	[SerializeField] private Image _progressBar;

	private Animator _animator;
	private string _nextSceneToLoad;

	public override void Awake()
	{
		base.Awake();
		_animator = GetComponent<Animator>();
	}

	public void AnimatorHandleNextSceneLoad() => LoadScene(_nextSceneToLoad);

	public void LoadScene(string sceneName)
	{
		_nextSceneToLoad = sceneName;
		_animator.Play(LOAD_ANIMATION);
	}

	public void HandleLoadScene() => LoadSceneAsync(_nextSceneToLoad);

	public async void LoadSceneAsync(string sceneName)
	{
		var scene = SceneManager.LoadSceneAsync(sceneName);
		scene.allowSceneActivation = false;

		do
		{
			await Task.Delay(100);
			_progressBar.fillAmount = scene.progress;
		} while (scene.progress < 0.9f);

		await Task.Delay(100);
		scene.allowSceneActivation = true;
	}

	public void PlayLoadedAnimation() => _animator.Play(LOADED_ANIMATION);
}
