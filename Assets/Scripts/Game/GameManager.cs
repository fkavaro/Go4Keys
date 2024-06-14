using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Implements context methods 
/// </summary>
public class GameManager : AStateController
{
    public static GameManager Instance;
    public event EventHandler<string> ButtonClicked;
    //[HideInInspector] public string result;

    #region STATES
    public GameMainMenuState mainMenuState = new();
    public GameOptionsMenuState optionsMenuState = new();
    public GameCreditsState creditsState = new();
    public GamePlayState playState = new();
    public GamePauseState pauseState = new();
    public GameEndState endState = new();
    #endregion

    #region BEHAVIOURS
    public PlayerCollectiblesUI playerCollectedUI;
    public PlayerStaminaUI playerStaminaUI;
    //public KeyAutosave autosave;
    #endregion

    #region GRAPHICS
    [Header("Collectibles")]
    [Tooltip("Contains all left collectibles icons in UI")]
    [SerializeField] GameObject leftIcons;
    [Tooltip("Contains all found collectibles icons in UI")]
    [SerializeField] GameObject foundIcons;

    [Header("Stamina bar")]
    [SerializeField] Slider staminaBar;

    //TUTORIAL
    #endregion

    public override void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public override void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            SetState(mainMenuState);
        }
        else
        {
            playerCollectedUI = new(leftIcons, foundIcons);
            playerStaminaUI = new(staminaBar);
            //autosave = new();

            SetState(playState);
        }
    }

    public void ClickButton(string buttonName)
    {
        ButtonClicked?.Invoke(this, buttonName);
    }
}
