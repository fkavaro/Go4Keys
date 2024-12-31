using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)] // Will run before any other script
public class InputManager : MonoBehaviour
{
    public static InputManager Instance; // Singleton
    PlayerInput playerInput; // InputActions script

    #region Events
    public delegate void StartTouch(Vector2 pos, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 pos, float time);
    public event EndTouch OnEndTouch;
    #endregion

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(gameObject);

        playerInput = new PlayerInput();
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    void Start()
    {
        playerInput.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playerInput.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        if (OnStartTouch != null)
            OnStartTouch(ScreenToWorld(Camera.main, playerInput.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.startTime);
    }

    void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        if (OnEndTouch != null)
            OnEndTouch(ScreenToWorld(Camera.main, playerInput.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.time);
    }

    public Vector2 PrimaryPosition()
    {
        return ScreenToWorld(Camera.main, playerInput.Touch.PrimaryPosition.ReadValue<Vector2>());
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    public static Vector3 ScreenToWorld(Camera camera, Vector3 pos)
    {
        pos.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(pos);
    }
}
