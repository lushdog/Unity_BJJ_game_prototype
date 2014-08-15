using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IdleState : State
{
    protected override void OnEnter (IState previousState)
    {
        //_bjjPlayer.DebugLog("IdleState OnEnter");
    }
    
    protected override void OnExit (IState nextState)
    {
        //_bjjPlayer.DebugLog("IdleState OnExit");
    }
    
    public override void Update ()
    {
        //_bjjPlayer.DebugLog("IdleState Update");
        if (_input.IsInputActive(Control.RightArm))
        {
            if (_bjjPlayer.PlayerNumber == 1)
            {
                _bjjPlayer.SetState(BjjState.Grab);
            }
        } 
    }
    
    public IdleState(BjjPlayer bjjPlayer, Dictionary<BjjState, State> subStates) : base(bjjPlayer, subStates)
    {
    }
}
