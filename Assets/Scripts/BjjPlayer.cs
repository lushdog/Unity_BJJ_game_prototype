using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using RootMotion.FinalIK;

[System.Serializable]
public enum BjjState { Null, Idle, Grab, PullingGuard, EnteringGuard };

[System.Serializable]
public class BjjPlayer : MonoBehaviour {

    public int PlayerNumber;
    public Dictionary<BjjState, IState> States;
    public BjjState CurrentState = BjjState.Null;
    public BjjPlayer Opponent;
    public BjjMovementManager MovementManager;
    public InputBinding InputBinding;
   
    public void DebugLog(string message)
    {
        Debug.Log("Player" + PlayerNumber + ":" + message);
    }

    public void SetState(BjjState state)  
    {
        if (CurrentState == BjjState.Null)
        {
            States[state].TransitionFrom(null);
        } 
        else
        {
            States[CurrentState].TransitionTo(States[state]);
        }
        
        CurrentState = state;
    }

    private void Awake()
    {
        MovementManager = new BjjMovementManager(this);

        InputBinding = new KeyboardInputBinding(PlayerNumber);

        IdleState idle = new IdleState(this, null);
        GrabbingState grab = new GrabbingState(this, null);
        PullGuardState pullingGuard = new PullGuardState(this, null);
        EnterGuardState enteringGuard = new EnterGuardState(this, null);
        States = new Dictionary<BjjState, IState> 
        {   {BjjState.Null, null}, 
            {BjjState.Idle, idle}, 
            {BjjState.Grab, grab}, 
            {BjjState.PullingGuard, pullingGuard}, 
            {BjjState.EnteringGuard, enteringGuard} 
        };
    }
	
    private void Start () 
    {

	}

   	private void Update () 
    {
        States[CurrentState].Update();
	}

	private void LateUpdate()  
    {
        MovementManager.Update();
	}


}