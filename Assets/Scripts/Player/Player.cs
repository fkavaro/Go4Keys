﻿using System;
using UnityEngine;

/// <summary>
/// Implements context and transitions between player states.
/// </summary>
public class Player : AStateController
{
    public static Player Instance;
    public event EventHandler<string> PlayerResult;

    #region STATES
    readonly PlayerRunState runState = new();
    readonly PlayerJumpState jumpState = new();
    readonly PlayerAtIntersectionState atIntersection = new();
    #endregion

    #region HABILITIES
    public EndlessRunner endlessRunner;
    public Railed railed;
    public Jumper jumper;
    public Turner turner;
    public Resilient resilient;
    public KeyCollecter keyCollecter;
    public Chased chased;
    #endregion

    #region PROPERTIES
    [Header("Movement")]
    [SerializeField] float forwardSpeed;
    [SerializeField] float railChangeSpeed;
    [SerializeField] float jumpForce;
    [Header("Mechanics")]
    [SerializeField] int keysToCollect;
    [SerializeField] float staminaLossPerStep;
    [SerializeField] float staminaLossPerJump;
    [SerializeField] float chaserResetDistance;
    #endregion

    public override void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Rigidbody rigidBody = GetComponent<Rigidbody>();
        Transform playerParent = transform.parent;

        endlessRunner = new(playerParent, forwardSpeed);
        railed = new(transform, railChangeSpeed);
        jumper = new(rigidBody, jumpForce);
        turner = new(playerParent);
        resilient = new(staminaLossPerStep, staminaLossPerJump);
        keyCollecter = new(keysToCollect);
        chased = new(chaserResetDistance);
    }

    public override void Start()
    {
        endlessRunner.Initialize(); // Detects obstacle checker
        railed.Initialize(); // Detects rails parent
        jumper.Initialize(); // Detects ground checker

        jumper.JumpEvent += Jump;
        jumper.GroundedEvent += Grounded;
        endlessRunner.AtIntersectionEvent += AtIntersection;
        turner.TurnedEvent += TurnPointSurpassed;
        keyCollecter.AllFoundEvent += Victory;
        chased.CaughtEvent += Caught;
        resilient.StaminaChangeEvent += Tired;

        SetState(runState);
    }

    public override void UpdateFrame()
    {
        endlessRunner.Update(); // Runs endlessly,
        resilient.LossPerStep(); // thus, loses stamina
        railed.Update(); // Changes rails
    }

    public override void OnTriggerEnter(Collider other)
    {
        endlessRunner.OnTriggerEnter(other); // Can enter an intersection
        resilient.OnTriggerEnter(other); // Can recover stamina
        keyCollecter.OnTriggerEnter(other); // Can find a collectible
        turner.OnTriggerEnter(other); // Set the center of intersection
        chased.OnTriggerEnter(other); // Can reset chaser position
    }

    public override void OnCollisionEnter(Collision collision)
    {
        chased.OnCollisionEnter(collision); // Can be caught
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Transform obstacleChecker = GameObject.Find("Player obstacle checker").transform;
        Transform groundChecker = GameObject.Find("Player ground checker").transform;
        Gizmos.DrawSphere(obstacleChecker.position, 0.4f);
        Gizmos.DrawSphere(groundChecker.position, 0.1f);
    }

    void Jump(object sender, EventArgs any)
    {
        SetState(jumpState);
    }

    void Grounded(object sender, EventArgs any)
    {
        SetState(runState);
    }

    void AtIntersection(object sender, EventArgs any)
    {
        SetState(atIntersection);
    }

    void TurnPointSurpassed(object sender, bool turned)
    {
        SetState(runState);
    }

    void Victory(object sender, EventArgs e)
    {
        PlayerResult?.Invoke(this, "Victory");
    }

    void Caught(object sender, EventArgs any)
    {
        PlayerResult?.Invoke(this, "Caught");
    }

    void Tired(object sender, float stamina)
    {
        if (stamina <= 0)
        {
            PlayerResult?.Invoke(this, "Tired");
        }
    }
}