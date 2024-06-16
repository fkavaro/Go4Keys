using UnityEngine;

/// <summary>
/// Implements common functionalities in all game states
/// </summary>
public abstract class AGameState : IState
{
    protected GameManager game;

    public virtual void Enter(AStateController controller)
    {
        game = (GameManager)controller;
        Enter();
    }
    public virtual void Enter() { }
    public virtual void Enter(AStateController controller, string info)
    {
        game = (GameManager)controller;
        Enter(info);
    }
    public virtual void Enter(string info) { }
    public virtual void UpdateFrame()
    {
        Update();
    }
    public virtual void Update() { }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnCollisionEnter(Collision collision) { }
    public virtual void Exit() { }
}
