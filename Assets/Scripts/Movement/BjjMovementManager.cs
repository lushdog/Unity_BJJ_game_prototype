using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

public class BjjMovementManager : MovementManager 
{	
    public BjjMovementManager(BjjPlayer player) : base(player)
    {
        
    }

    public void PullGuard() 
    {
        Pose(_player.transform.Find("Positions/GuardBottom").transform);
        _player.SetState(BjjState.Idle);
    }
    
    public void EnterGuard() 
    {
        Pose(_player.transform.Find("Positions/GuardTop").transform);
        _player.SetState(BjjState.Idle);
    }

    public void Grab(BodyPoint grabbingPoint, BodyPoint grabbedPoint, BodyPointLocation location)
    {
        if (!_is.IsInInteraction(grabbingPoint.Effector))
        {
            Debug.Log("Start interaction...");
            InteractionObject interactionObject = grabbedPoint.GetInteractionObject(location);
            grabbingPoint.MovementType = MovementType.Interaction;
            _is.StartInteraction(grabbingPoint.Effector, interactionObject, true);
            Debug.DrawLine(grabbingPoint.GameObject.transform.position, interactionObject.transform.position, Color.white);
        }
        else if (_is.IsPaused(grabbingPoint.Effector))
        {
            Debug.Log("Interaction paused...");
            grabbingPoint.MovementType = MovementType.None;
            _player.SetState(BjjState.Idle);
        } 
        else
        {
            Debug.Log("Interaction update...");
        }

    }
}
