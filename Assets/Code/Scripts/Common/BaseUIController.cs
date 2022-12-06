using System.Collections.Generic;
using UnityEngine;

public class BaseUIController : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject> _views;

    protected ScenesController _scenesController;

    protected void Awake() => _scenesController = GameObject.FindObjectOfType<ScenesController>();

    protected void SetViewActive(string viewName) => _views.ForEach(view => view.SetActive(view.name.Equals(viewName)));
}
