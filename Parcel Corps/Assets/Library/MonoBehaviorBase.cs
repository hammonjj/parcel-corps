using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum LogLevel {
    Debug,
    Info,
    Warning,
    Error,
    None
}

public abstract class MonoBehaviourBase : MonoBehaviour {
    [Header("Debug Settings")]
    [Tooltip("Enable to draw gizmos for this object.")]
    public bool showGizmos = false;

    [Tooltip("Minimum log level to output to the console.")]
    public LogLevel logLevel = LogLevel.Info;

    private string _cachedObjectName;
    private int _debugId;
    protected MessageBus _sceneMessageBus;

    protected virtual void Awake()
    {
        _cachedObjectName = gameObject.name;
        _debugId = GetInstanceID();
    }

    protected virtual void OnEnable()
    {
        if (_sceneMessageBus == null) {
            _sceneMessageBus = GameObject.FindWithTag("SceneMessageBus")?.GetComponent<MessageBus>();
        }

        if (_sceneMessageBus == null) {
            LogWarning("SceneLevelMessageBus not found in the scene.");
        }
    }

    protected void Log(LogLevel level, string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) {
        if (logLevel > level) {
            return;
        }

        string formatted = FormatLog(level.ToString().ToUpper(), message, filePath, lineNumber);

        switch (level) {
            case LogLevel.Debug: {
                Debug.Log(formatted);
                break;
            }
            case LogLevel.Info: {
                Debug.Log(formatted);
                break;
            }
            case LogLevel.Warning: {
                Debug.LogWarning(formatted);
                break;
            }
            case LogLevel.Error: {
                Debug.LogError(formatted);
                break;
            }
        }
    }

    protected void LogDebug(string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) {
        Log(LogLevel.Debug, message, filePath, lineNumber);
    }

    protected void LogInfo(string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) {
        Log(LogLevel.Info, message, filePath, lineNumber);
    }

    protected void LogWarning(string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) {
        Log(LogLevel.Warning, message, filePath, lineNumber);
    }

    protected void LogError(string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) {
        Log(LogLevel.Error, message, filePath, lineNumber);
    }

    private string FormatLog(string level, string msg, string filePath, int line) {
        string fileName = Path.GetFileName(filePath);
        return $"{fileName} ({line}): [{level}] [{_cachedObjectName}#{_debugId}] {msg}";
    }

    protected virtual void OnDrawGizmos() {
        if (showGizmos) {
            DrawGizmosSafe();
        }
    }

    protected virtual void DrawGizmosSafe() { }
}
