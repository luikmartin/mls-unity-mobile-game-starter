public static class Utils
{
    public static T ParseEnum<T>(string value) => (T)System.Enum.Parse(typeof(T), value, true);
}
