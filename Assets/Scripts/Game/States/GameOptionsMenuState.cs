using UnityEngine;

/// <summary>
/// Handles music and effects volume, as well as tutorial visibility. Can switch to main menu state.
/// </summary>
public class GameOptionsMenuState : AState
{
    GameObject UI;
    bool alreadyEntered = false;

    public override void Enter()
    {
        UI = GameObject.Find("UI");
        UI = UI.transform.Find("Options menu UI").gameObject;
        UI.SetActive(true);

        if (!alreadyEntered) 
        {
            GameManager.Instance.tutorialToggler.Initialize(); // Finds buttons
            alreadyEntered = true;
        }
    }

    public override void Exit()
    {
        UI.SetActive(false);
    }
}