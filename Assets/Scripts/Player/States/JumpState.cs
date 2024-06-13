using System;
using UnityEngine;

/// <summary>
/// Jumps once while still moving forward endlessly and changing rail.
/// Can enter an intersection or be caught.
/// </summary>
public class JumpState : APlayerState
{
    bool hasSurpassedTurnPoint = true;
    bool isCaught = false;
    bool eventsSubscribed = false;

    public override void Enter(AStateController controller)
    {
        player = (Player) controller;
        player.jumper.Jump();// Jumps
        player.resilient.Jumps();// Jumped, thus, loses a lot of stamina

        if (!eventsSubscribed)// Subscribe just once
        {
            player.endlessRunner.AtIntersectionEvent += AtIntersection;
            player.turner.TurnedEvent += TurnPointSurpassed;
            player.chaserResetter.CaughtEvent += Caught;

            eventsSubscribed = true;
        }
    }

    public override void Update()
    {
        player.endlessRunner.Update(); // Continues moving forward
        player.railed.Update(); // Change rails
        
        Exit(); // Checks exit
    }

    public override void OnTriggerEnter(Collider other)
    {
        player.endlessRunner.OnTriggerEnter(other); // Can enter an intersection
        player.resilient.OnTriggerEnter(other); // Can recover stamina
        player.collecter.OnTriggerEnter(other); // Can find a collectible
        player.chaserResetter.OnTriggerEnter(other); // Can reset chaser position
    }

    public override void Exit()
    {
        if (player.jumper.IsGrounded())
        {
            player.SetState(player.runState);
        }
        if (!hasSurpassedTurnPoint)
        {
            player.SetState(player.atIntersection);
        }
        else if (player.jumper.IsGrounded())
        {
            player.SetState(player.runState);
        }
        else if (isCaught)
        {
            //isCaught = false; No need bc won't return
            player.SetState(player.caughtState);
        }
    }

    void AtIntersection(object sender, EventArgs any)
    {
        hasSurpassedTurnPoint = false;
    }
    void TurnPointSurpassed(object sender, bool turned)
    {
        hasSurpassedTurnPoint = true;
    }

    void Caught(object sender, EventArgs any)
    {
        isCaught = true;
    }
}
