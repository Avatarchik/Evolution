using UnityEngine;

public static class Log {
    public static void Info(object message)
    {
        Debug.Log(message);
    }

    public static void Warning(object message)
    {
        Debug.LogWarning(message);
    }

    public static void Error(object message)
    {
        Debug.LogError(message);
    }
}
