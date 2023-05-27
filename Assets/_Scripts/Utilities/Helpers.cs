using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helpers
{
    private static Camera _camera;
    public static Camera Camera
    {
        get
        {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }

    private static readonly Dictionary<float, WaitForSeconds> _waitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if (_waitDictionary.TryGetValue(time, out var wait)) return wait;
        _waitDictionary[time] = new WaitForSeconds(time);

        return _waitDictionary[time];
    }

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    public static bool IsOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);

        return _results.Count > 0;
    }

    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(element, element.position, Camera, out var result);

        return result;
    }

	public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
	{
		position.z = camera.nearClipPlane;

		return camera.ScreenToWorldPoint(position);
	}

	public static void DestroyChildren(this Transform parent)
    {
        foreach (Transform child in parent) Object.Destroy(child.gameObject);
    }

	public static T Find<T>() where T : MonoBehaviour
	{
		var result = Object.FindObjectOfType<T>();

		if (result == null)
		{
			Debug.LogErrorFormat("Failed to find MonoBehaviour of type '{0}'", typeof(T));
		}
		return result;
	}

	public static CONFIG_TYPE LoadConfig<CONFIG_TYPE>(string path) => FromJson<CONFIG_TYPE>(File.ReadAllText(path));

	public static T ParseEnum<T>(string value) => (T)System.Enum.Parse(typeof(T), value, true);

	public static T FromJson<T>(string json) => JsonUtility.FromJson<T>(json);

	public static string ToJson<T>(T obj) => JsonUtility.ToJson(obj, true);
}
