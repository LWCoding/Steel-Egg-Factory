using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{

    public static ScoreController Instance;
    public int Score;
    [SerializeField] private TextMeshProUGUI _scoreText;

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
    }

    // Set the score to a specified value.
    public void SetScore(int score)
    {
        Score = score;
        UpdateScoreText();
    }

    // Increment the score by a specified value.
    public void IncrementScore(int score)
    {
        Score += score;
        UpdateScoreText();
    }

    // Set the score text. If it's less than ten, add a zero behind the digit.
    private void UpdateScoreText()
    {
        _scoreText.text = (Score < 10) ? "0" + Score.ToString() : Score.ToString();
    }

}
