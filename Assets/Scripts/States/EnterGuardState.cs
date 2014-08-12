using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnterGuardState : State
{
    protected override void OnEnter (IState previousState)
    {
        _bjjPlayer.DebugLog("EnterGuardState OnEnter");
    }
    
    protected override void OnExit (IState nextState)
    {
        _bjjPlayer.DebugLog("EnterGuardState OnExit");
    }
    
    public override void Update ()
    {
        _bjjPlayer.DebugLog("EnterGuardState Update");
        _bjjPlayer.MovementManager.EnterGuard();
    }
    
    public EnterGuardState(BjjPlayer bjjPlayer, Dictionary<BjjState, State> subStates) : base(bjjPlayer, subStates)
    {
    }
}
