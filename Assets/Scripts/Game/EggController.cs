using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour
{

    public static EggController Instance;
    public LayerMask EggLayer;
    [Header("Sound Assignments")]
    public AudioClip eggInBasketSFX;
    private bool _isClickingEgg;
    private bool _isTouchingBasket;
    private Vector3 _startingPosition;
    private Rigidbody2D _rb;

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
        _startingPosition = transform.position;
        _isClickingEgg = false;
        _isTouchingBasket = false;
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetIsEggInteractable(bool isEggInteractable)
    {
        StartCoroutine(SetIsEggInteractableCoroutine(isEggInteractable));
    }

    private IEnumerator SetIsEggInteractableCoroutine(bool isEggInteractable)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() =>
        {
            return transform.position == _startingPosition;
        });
        gameObject.layer = (isEggInteractable) ? 3 : 2; // Egg : IgnoreRaycast
    }

    // Shoot a raycast when the left button is clicked, if the chicken is standing.
    // If the egg is hit, perform some logic with it.
    private void Update()
    {
        if (GameController.Instance.State != GameState.PLAYING) { return; }
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get mouse position in the world space
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, EggLayer); // Shoot a raycast at the mouse position
        if (hit.collider != null && hit.collider.gameObject == this.gameObject)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isClickingEgg = true;
            }
            ChickenController.Instance.IncrementMouseOverTimeWhileStanding();
        }
        // If the left mouse button is released, the egg is no longer being clicked.
        // Make it fall due to gravity.
        if (_isClickingEgg && Input.GetMouseButtonUp(0))
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _isClickingEgg = false;
        }
        if (!_isClickingEgg && (transform.position.y < -4.5f || _isTouchingBasket))
        {
            // If touching the basket, increment score by one.
            if (_isTouchingBasket)
            {
                SoundManager.Instance.PlayOneShot(eggInBasketSFX, 0.5f);
                ScoreController.Instance?.IncrementScore(1);
            }
            // Return the egg to the original position.
            _rb.velocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            transform.position = _startingPosition;
            _isTouchingBasket = false;
            _isClickingEgg = false;
        }
        // If the mouse is dragging this egg, move the egg to the mouse.
        if (Input.GetMouseButton(0) && _isClickingEgg)
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    // If touching the basket, set _isTouchingBasket to true.
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Basket")
        {
            _isTouchingBasket = true;
        }
    }

    // If touching the basket, set _isTouchingBasket to false.
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Basket")
        {
            _isTouchingBasket = false;
        }
    }

}
