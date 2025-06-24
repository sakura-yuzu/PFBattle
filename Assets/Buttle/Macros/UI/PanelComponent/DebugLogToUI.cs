using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugLogToUI : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    private string logMessages = "";

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logMessages += logString + "\n";
        debugText.text = logMessages;
    }
}