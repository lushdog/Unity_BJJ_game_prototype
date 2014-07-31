using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IState  
{
    void TransitionTo(IState newState);
    void TransitionFrom(IState oldState);
    void Update();
}

public abstract class StateBase : IState
{
    protected List<IState> _subStates;
    protected DummyBehaviour _dummy;

    public void TransitionTo(IState nextState)
    {
        OnExit(nextState);
        nextState.TransitionFrom(this);
    }

    public void TransitionFrom(IState previousState)
    {
        OnEnter(previousState);
    }

    public StateBase(DummyBehaviour dummy)
    {
        _dummy = dummy;
    }

    public abstract void Update();
    protected abstract void OnEnter(IState previousState);
    protected abstract void OnExit (IState nextState);

}

public class IdleState : StateBase
{
    protected override void OnEnter (IState previousState)
    {
        _dummy.DebugLog("IdleState OnEnter");
    }

    protected override void OnExit (IState nextState)
    {
        _dummy.DebugLog("IdleState OnExit");
    }

    public override void Update ()
    {
        //_dummy.DebugLog("IdleState Update");
    }

    public IdleState(DummyBehaviour dummy) : base (dummy)
    {
    }
}

public class GrabbingState : StateBase
{
    protected override void OnEnter (IState previousState)
    {
        _dummy.DebugLog("GrabbingState OnEnter");
    }
    
    protected override void OnExit (IState nextState)
    {
        _dummy.DebugLog("GrabbingState OnExit");
    }
    
    public override void Update ()
    {
        //_dummy.DebugLog("GrabbingState Update");
    }

    public GrabbingState(DummyBehaviour dummy) : base (dummy)
    {
    }
}

public class PullingGuard : StateBase
{
    protected override void OnEnter (IState previousState)
    {
        _dummy.DebugLog("PullingGuard OnEnter");
    }
    
    protected override void OnExit (IState nextState)
    {
        _dummy.DebugLog("PullingGuard OnExit");
    }
    
    public override void Update ()
    {
        //_dummy.DebugLog("PullingGuard Update");
        _dummy.PullGuard();

    }

    public PullingGuard(DummyBehaviour dummy) : base (dummy)
    {
    }
}

public class EnteringGuard : StateBase
{
    protected override void OnEnter (IState previousState)
    {
       _dummy.DebugLog("EnteringGuard OnEnter");
    }
    
    protected override void OnExit (IState nextState)
    {
        _dummy.DebugLog("EnteringGuard OnExit");
    }
    
    public override void Update ()
    {
        //_dummy.DebugLog("EnteringGuard Update");
        _dummy.EnterGuard();
    }
    
    public EnteringGuard(DummyBehaviour dummy) : base (dummy)
    {
    }
}

