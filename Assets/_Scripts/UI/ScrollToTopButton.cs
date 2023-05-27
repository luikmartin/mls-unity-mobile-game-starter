using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollToTopButton : MonoBehaviour
{
	[SerializeField]
	private ScrollRect _scrollRect;
	[SerializeField]
	private float _smoothness = 0.01f;
	[SerializeField]
	private Button _button;


	private void Awake() => _button.gameObject.SetActive(false);

	public void ScrollToTop() => StartCoroutine(SmoothScrollToTop());

	private IEnumerator SmoothScrollToTop()
	{
		while (_scrollRect.verticalNormalizedPosition < 1)
		{
			_scrollRect.verticalNormalizedPosition += _smoothness;
			yield return null;
		}
		_scrollRect.verticalNormalizedPosition = 1;
	}
}
