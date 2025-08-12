using UnityEngine;
using TMPro;

public class RaceTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;

    private float _raceTime;
    private bool _isRunning;

    void Update()
    {
        if (_isRunning)
        {
            _raceTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void StartTimer()
    {
        _raceTime = 0f;
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void ResetTimer()
    {
        _raceTime = 0f;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(_raceTime / 60f);
        int seconds = Mathf.FloorToInt(_raceTime % 60f);
        int milliseconds = Mathf.FloorToInt((_raceTime * 1000f) % 1000f);

        _timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }
}

