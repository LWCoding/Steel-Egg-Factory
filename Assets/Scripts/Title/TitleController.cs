using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleController : MonoBehaviour
{

    [Header("Object Assignments")]
    public Image volumeButtonImage;
    public TextMeshProUGUI highScoreText;
    [Header("Sprite Assignments")]
    public Sprite volumeButtonOnSprite;
    public Sprite volumeButtonOffSprite;
    [Header("Music Assignment")]
    public AudioClip TitleMusic;

    private void Start()
    {
        UpdateSound();
        UpdateHighScoreText();
        SoundManager.Instance?.PlayMusic(TitleMusic, 0.4f);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Toggles whether the sound should or should not be playing.
    public void ToggleSound()
    {
        SoundManager.Instance.ToggleSound();
        UpdateSound();
    }

    // Updates the sprite of the volume button, depending on whether
    // the SoundManger's audioSource is muted or not.
    private void UpdateSound()
    {
        if (!SoundManager.Instance.IsMuted)
        {
            volumeButtonImage.sprite = volumeButtonOnSprite;
        }
        else
        {
            volumeButtonImage.sprite = volumeButtonOffSprite;
        }
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
    }

}
