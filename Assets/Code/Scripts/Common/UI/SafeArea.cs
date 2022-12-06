using UnityEngine;
using UnityEngine.UI;

public class SafeArea : MonoBehaviour
{
    // source: https://stackoverflow.com/questions/63113590/unity-how-to-get-the-coordinates-of-the-safearea-rect
    private CanvasScaler _canvasScaler;
    private float _bottomUnits, _topUnits;

    private void Awake()
    {
        _canvasScaler = GetComponentInParent<CanvasScaler>();
    }

    private void Start()
    {
        ApplyVerticalSafeArea();
    }
#if UNITY_EDITOR
    // For testing purposes only, foe example if when simulator device changes
    private void Update()
    {
        ApplyVerticalSafeArea();
    }
#endif
    private void ApplyVerticalSafeArea()
    {
        var bottomPixels = Screen.safeArea.y;
        var topPixel = Screen.currentResolution.height - (Screen.safeArea.y + Screen.safeArea.height);

        var bottomRatio = bottomPixels / Screen.currentResolution.height;
        var topRatio = topPixel / Screen.currentResolution.height;

        var referenceResolution = _canvasScaler.referenceResolution;
        _bottomUnits = referenceResolution.y * bottomRatio;
        _topUnits = referenceResolution.y * topRatio;

        var rectTransform = GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, _bottomUnits);
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -_topUnits);
    }
}
