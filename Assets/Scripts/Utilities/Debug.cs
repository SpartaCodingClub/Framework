using System.Diagnostics;

public class Debug
{
    private const string DEBUG = nameof(DEBUG);

    [Conditional(DEBUG)]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [Conditional(DEBUG)]
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    [Conditional(DEBUG)]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }
}