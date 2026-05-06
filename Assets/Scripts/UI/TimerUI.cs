using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    /// --- Members ---
    [SerializeField] TMP_Text timerText;


    // --- Methods ---
    private void OnEnable()
    {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnSecondElapsed += UpdateTimerDisplay;
        }
        else
        {
            Debug.LogWarning("GameManager instance not found. TimerUI will not update.");
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnSecondElapsed -= UpdateTimerDisplay;
        }
    }

    private void Start()
    {
        // Initialize the timer display
        UpdateTimerDisplay();
    }


    // --- Event Handlers ---
    private void UpdateTimerDisplay()
    {
        if (GameManager.Instance != null)
        {
            int totalSeconds = Mathf.FloorToInt(GameManager.Instance.ElapsedTime);
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;
            timerText.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
    }
}
