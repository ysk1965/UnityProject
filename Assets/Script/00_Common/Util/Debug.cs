
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Internal;

public static class Debug
{
    public static bool isDebugBuild
    {
        get => UnityEngine.Debug.isDebugBuild;
    }

    public static UnityEngine.ILogger unityLogger
    {
        get => UnityEngine.Debug.unityLogger;
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogFormat(LogType logType, LogOption logOptions, UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(context, format, args);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogColor(object message, string color = "yellow")
    {
        UnityEngine.Debug.Log("<color='" + color + "'>" + message + "</color>");
    }


    // [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    // [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogError(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(format, args);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message.ToString());
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message, Object context)
    {
        UnityEngine.Debug.LogWarning(message.ToString(), context);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition)
    {
        if (!condition) throw new System.Exception();
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, Object context)
    {
        UnityEngine.Debug.Assert(condition, context);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, object context)
    {
        UnityEngine.Debug.Assert(condition, context);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogAssertion(string message)
    {
        UnityEngine.Debug.LogAssertion(message);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogException(System.Exception e)
    {
        UnityEngine.Debug.LogException(e);
    }
}