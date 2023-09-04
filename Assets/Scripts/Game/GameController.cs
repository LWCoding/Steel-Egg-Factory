using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState
{
    PLAYING, PAUSED, GAME_OVER
}

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    public GameState State;
    [Header("Object Assignments")]
    public TextMeshProUGUI _gameOverScoreText;
    public Animator _gameOverAnimator;
    [Header("Music Assignments")]
    public AudioClip GameMusic;
    [HideInInspector] public UnityEvent OnGameStart;

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

    // This is all logic that should run when the game starts. 
    private void Start()
    {
        TimerController.Instance?.StartTimer(30); // TODO: Make time vary!
        ScoreController.Instance?.SetScore(0);
        SoundManager.Instance?.PlayMusic(GameMusic, 0.4f);
        OnGameStart.Invoke();
    }

    // Change the game's state and invoke the state change event.
    public void SetGameState(GameState newGameState)
    {
        State = newGameState;
        switch (newGameState)
        {
            case GameState.GAME_OVER:
                StartCoroutine(RenderGameOverCoroutine());
                break;
        }
    }

    // Play the animation showing the player's final stats and then bring them
    // back to the title screen.
    private IEnumerator RenderGameOverCoroutine()
    {
        _gameOverScoreText.text = ScoreController.Instance.Score.ToString();
        _gameOverAnimator.Play("ShowStats");
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() =>
        {
            return !IsPlaying(_gameOverAnimator);
        });
        UpdateHighScoreIfApplicable();
        SceneManager.LoadScene("Title");
    }

    private void UpdateHighScoreIfApplicable()
    {
        int currScore = ScoreController.Instance.Score;
        if (currScore > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", currScore);
        }
    }

    private bool IsPlaying(Animator anim)
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
    }

}