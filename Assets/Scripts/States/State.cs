using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class State : IState
{
    protected Dictionary<BjjState, State> _subStates;
    protected BjjPlayer _bjjPlayer;
    protected InputBinding _input;

    public void TransitionTo(IState nextState)
    {
        OnExit(nextState);
        nextState.TransitionFrom(this);
    }
    
    public void TransitionFrom(IState previousState)
    {
        OnEnter(previousState);
    }
    
    public State(BjjPlayer bjjPlayer, Dictionary<BjjState, State> subStates)
    {
        _bjjPlayer = bjjPlayer;
        _subStates = subStates;
        _input = bjjPlayer.InputBinding;
    }

    //TODO: support sub-states in Update()
    public abstract void Update();
    protected abstract void OnEnter(IState previousState);
    protected abstract void OnExit (IState nextState);
    
}
