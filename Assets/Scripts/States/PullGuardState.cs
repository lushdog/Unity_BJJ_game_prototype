using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PullGuardState : State
{
    protected override void OnEnter (IState previousState)
    {
        //_bjjPlayer.DebugLog("PullingGuard OnEnter");
    }
    
    protected override void OnExit (IState nextState)
    {
        //_bjjPlayer.DebugLog("PullingGuard OnExit");
    }
    
    public override void Update ()
    {
        //_bjjPlayer.DebugLog("PullingGuard Update");
        _bjjPlayer.MovementManager.PullGuard();
    }
    
    public PullGuardState(BjjPlayer bjjPlayer, Dictionary<BjjState, State> subStates) : base(bjjPlayer, subStates)
    {
    }
}

