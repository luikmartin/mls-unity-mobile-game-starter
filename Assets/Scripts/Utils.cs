using System.IO;
using UnityEngine;

public static class Utils
{
	public static CONFIG_TYPE LoadConfig<CONFIG_TYPE>(string path) => FromJson<CONFIG_TYPE>(File.ReadAllText(path));

	public static T ParseEnum<T>(string value) => (T)System.Enum.Parse(typeof(T), value, true);

	public static T FromJson<T>(string json) => JsonUtility.FromJson<T>(json);

	public static string ToJson<T>(T obj) => JsonUtility.ToJson(obj, true);
}
