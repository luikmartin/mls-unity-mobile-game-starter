using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	protected List<GameObject> _views;

	private ScenesManager _scenesManager;


	public virtual void Awake() => _scenesManager = FindObjectOfType<ScenesManager>();

	public void SetViewActive(string viewName) => _views.ForEach(view => view.SetActive(view.name.Equals(viewName)));

	public void LoadGameScene() => _scenesManager.LoadScene(Constants.GAME_SCENE);

	public void LoadMenuScene() => _scenesManager.LoadScene(Constants.MENU_SCENE);
}
