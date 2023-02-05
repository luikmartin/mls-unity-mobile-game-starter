using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour, IUIController
{
	[SerializeField]
	protected List<GameObject> _views;

	protected ScenesController _scenesController;

	public virtual void Awake() => _scenesController = FindObjectOfType<ScenesController>();

	public void SetViewActive(string viewName) => _views.ForEach(view => view.SetActive(view.name.Equals(viewName)));
}
