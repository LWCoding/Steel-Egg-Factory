using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChickenState
{
    SITTING, STANDING
}

public class ChickenController : MonoBehaviour
{

    public static ChickenController Instance;
    public ChickenState ChickenState;
    [Header("Sound Assignments")]
    public AudioClip AccidentallyClickChickenSFX;
    public AudioClip ChickenStandUpSFX;
    public AudioClip ChickenSitDownSFX;
    [SerializeField] private Sprite _sittingChickenSprite;
    [SerializeField] private Sprite _standingChickenSprite;
    private SpriteRenderer _spriteRenderer;
    private bool _isMouseOver = false;
    private float _mouseOverTimeWhileStanding = 0;
    private const float MOUSE_TIME_UNTIL_TRANSPARENT = 0.35f;

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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GameController.Instance.OnGameStart.AddListener(() => { StartCoroutine(RandomlyModifyChickenStateCoroutine()); });
    }

    // Set the chicken's new state.
    public void SetChickenState(ChickenState chickenState)
    {
        ChickenState = chickenState;
        switch (chickenState)
        {
            case ChickenState.SITTING:
                _spriteRenderer.sprite = _sittingChickenSprite;
                SoundManager.Instance.PlayOneShot(ChickenSitDownSFX, 0.3f);
                EggController.Instance.SetIsEggInteractable(false);
                break;
            case ChickenState.STANDING:
                _spriteRenderer.sprite = _standingChickenSprite;
                SoundManager.Instance.PlayOneShot(ChickenStandUpSFX, 0.35f);
                EggController.Instance.SetIsEggInteractable(true);
                break;
        }
    }

    // Randomly modify the chicken state from standing to sitting and
    // vice versa throughout a random time interval.
    public IEnumerator RandomlyModifyChickenStateCoroutine()
    {
        float randomWaitTime = Random.Range(1.5f, 4);
        yield return new WaitForSeconds(randomWaitTime);
        SetChickenState((ChickenState == ChickenState.SITTING) ? ChickenState.STANDING : ChickenState.SITTING);
        if (GameController.Instance.State == GameState.GAME_OVER) { yield break; }
        StartCoroutine(RandomlyModifyChickenStateCoroutine());
    }

    public void Update()
    {
        switch (ChickenState)
        {
            case ChickenState.SITTING:
                if (_spriteRenderer.color.a != 1)
                {
                    _spriteRenderer.color = new Color(1, 1, 1, 1);
                    _mouseOverTimeWhileStanding = 0;
                }
                break;
            case ChickenState.STANDING:
                if (_isMouseOver)
                {
                    // If the mouse is over, start accumulating time. This will make the 
                    // chicken more transparent.
                    if (_mouseOverTimeWhileStanding < MOUSE_TIME_UNTIL_TRANSPARENT)
                    {
                        _mouseOverTimeWhileStanding += Time.deltaTime;
                    }
                }
                else
                {
                    // If the mouse isn't over, reduce time until the total time is back to 0.
                    if (_mouseOverTimeWhileStanding > 0)
                    {
                        _mouseOverTimeWhileStanding -= Time.deltaTime;
                    }
                }
                // Adjust the sprite renderer's alpha value depending on the cursor time over the chicken.
                _spriteRenderer.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0.5f), _mouseOverTimeWhileStanding / MOUSE_TIME_UNTIL_TRANSPARENT);
                break;
        }
    }

    // Can be called by the egg sprite to make the chicken more transparent.
    public void IncrementMouseOverTimeWhileStanding()
    {
        if (_mouseOverTimeWhileStanding < MOUSE_TIME_UNTIL_TRANSPARENT)
        {
            _mouseOverTimeWhileStanding += Time.deltaTime * 2; // Double deltaTime to combat decrease
        }
    }

    public void OnMouseDown()
    {
        // If the chicken is still sitting, punish the player for clicking.
        if (ChickenState == ChickenState.SITTING)
        {
            TimerController.Instance.DecrementTimer(2);
            SoundManager.Instance.PlayOneShot(AccidentallyClickChickenSFX, 0.5f);
        }
    }

    public void OnMouseEnter()
    {
        if (GameController.Instance.State != GameState.PLAYING) { return; }
        _isMouseOver = true;
    }

    public void OnMouseExit()
    {
        if (GameController.Instance.State != GameState.PLAYING) { return; }
        _isMouseOver = false;
    }

}
