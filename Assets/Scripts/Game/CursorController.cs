using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    [SerializeField] private Texture2D _cursorIdleTexture;
    [SerializeField] private Texture2D _cursorHoldTexture;
    private Vector2 _cursorHotspot;

    // The cursor hotspot should be centered, based on the nature of the texture.
    private void Awake()
    {
        _cursorHotspot = new Vector2(_cursorIdleTexture.width / 2, _cursorIdleTexture.height / 2);
    }

    // Change the cursor to the idle texture from the start, just in case.
    private void Start()
    {
        Cursor.SetCursor(_cursorIdleTexture, _cursorHotspot, CursorMode.Auto);
    }

    // If the user clicks the mouse button down, change the cursor to the held
    // texture. When the user lifts it up, change the cursor to the idle texture.
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(_cursorHoldTexture, _cursorHotspot, CursorMode.Auto);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(_cursorIdleTexture, _cursorHotspot, CursorMode.Auto);
        }
    }

}
