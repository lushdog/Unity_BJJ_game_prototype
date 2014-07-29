using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IState  
{
    List<IState> SubStates 
    {
        get;
        set;
    } 

    void TransitionTo(IState state);
    void TransitionFrom(IState state);
    void Update();
}

public abstract class StateBase : IState
{
    public List<IState> SubStates 
    {
        get;
        set;
    } 

    public void TransitionTo(IState state)
    {
        OnExit(state);
        state.TransitionFrom(this);
    }

    public void TransitionFrom(IState state)
    {
        OnEnter(state);
    }

    protected abstract void OnEnter(IState previousState);
    protected abstract void OnExit (IState nextState);
}

public class IdleState : StateBase
{
    protected override void OnEnter (IState previousState)
    {
        throw new System.NotImplementedException ();
    }

    protected override void OnExit (IState nextState)
    {
        throw new System.NotImplementedException ();
    }

    public void Update ()
    {
        throw new System.NotImplementedException ();
    }
}

