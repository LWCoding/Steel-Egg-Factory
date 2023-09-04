using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{

    public static TimerController Instance;
    [SerializeField] private GameObject _timerBGObject;
    [SerializeField] private GameObject _timerFillObject;
    [SerializeField] private TextMeshProUGUI _timerText;
    private int _secondsRemaining = 0;
    private int _totalSeconds = 0;
    private float _maxTimerXScale = 0;
    private IEnumerator _timerCoroutine = null;

    // Make this a singleton instance; if there is another instance of this
    // script already running, then destroy this script.
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        _maxTimerXScale = _timerBGObject.transform.localScale.x;
    }

    // Start making the timer run down from a pre-set amount of seconds.
    // When the timer reaches zero, this sets the GAME OVER state in the GameController.
    public void StartTimer(int seconds)
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }
        _totalSeconds = seconds;
        _secondsRemaining = seconds;
        UpdateTimerVisuals();
        _timerCoroutine = DecrementTimerPerSecondCoroutine();
        StartCoroutine(_timerCoroutine);
    }

    public void DecrementTimer(int seconds)
    {
        _secondsRemaining -= seconds;
        // If the timer is at or less than zero, end the game. Or else, continue decrementing.
        if (_secondsRemaining <= 0)
        {
            _secondsRemaining = 0;
            GameController.Instance?.SetGameState(GameState.GAME_OVER);
        }
        UpdateTimerVisuals();
    }

    // Coroutine that decrements the timer by one every second until the time is zero.
    private IEnumerator DecrementTimerPerSecondCoroutine()
    {
        // Wait one second and then decrement the timer by one.
        yield return new WaitForSeconds(1);
        DecrementTimer(1);
        // If the timer is greater than zero, continue decrementing.
        if (_secondsRemaining > 0)
        {
            StartCoroutine(DecrementTimerPerSecondCoroutine());
        }
    }

    // Update the UI timer text as well as the bar.
    private void UpdateTimerVisuals()
    {
        UpdateTimerBar();
        UpdateTimerText();
    }

    // Update the scale of the timer.
    private void UpdateTimerBar()
    {
        _timerFillObject.transform.localScale = new Vector3(_maxTimerXScale * ((float)_secondsRemaining / _totalSeconds), 1, 1);
    }

    // Set the text of the UI timer.
    private void UpdateTimerText()
    {
        _timerText.text = _secondsRemaining.ToString();
    }

}
