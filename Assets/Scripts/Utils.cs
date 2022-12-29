using UnityEngine;

public static class Utils
{
    public static T ParseEnum<T>(string value) => (T)System.Enum.Parse(typeof(T), value, true);

    public static T FromJson<T>(string json) => JsonUtility.FromJson<T>(json);

    public static string ToJson<T>(T obj) => JsonUtility.ToJson(obj);
}
