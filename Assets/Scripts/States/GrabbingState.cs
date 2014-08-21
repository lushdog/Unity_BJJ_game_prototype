using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabbingState : State
{
    protected override void OnEnter (IState previousState)
    {
        //_bjjPlayer.DebugLog("GrabbingState OnEnter");
    }
    
    protected override void OnExit (IState nextState)
    {
        //_bjjPlayer.DebugLog("GrabbingState OnExit");
    }
    
    public override void Update ()
    {
        //_bjjPlayer.DebugLog("GrabbingState Update");
        _bjjPlayer.MovementManager.Grab(_bjjPlayer.MovementManager.rightHand, _bjjPlayer.Opponent.MovementManager.leftHand, BodyPointLocation.Front);
    }
    
    public GrabbingState(BjjPlayer bjjPlayer, Dictionary<BjjState, State> subStates) : base(bjjPlayer, subStates)
    {
        
    }
}
